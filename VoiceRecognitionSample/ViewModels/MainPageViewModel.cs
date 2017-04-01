using System.ComponentModel;
using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using VoiceRecognitionSample.Models;

namespace VoiceRecognitionSample.ViewModels
{
	/// <summary>
	/// MainPage.xamlに対応するViewModel
	/// </summary>
	public class MainPageViewModel : BindableBase, INavigationAware
	{
		#region Constants

		/// <summary>
		/// 音声認識の開始・停止ボタンのテキスト
		/// </summary>
		private const string BUTTON_TEXT_START = "開始";
		private const string BUTTON_TEXT_STOP = "停止";

		#endregion

		#region Properties, Variables

		/// <summary>
		/// 音声認識の結果テキスト
		/// </summary>
		private string _recognizedText = string.Empty;
		public string RecognizedText
		{
			get { return _recognizedText; }
			protected set { SetProperty(ref _recognizedText, value); }
		}

		/// <summary>
		/// 音声認識の開始・停止ボタンの表記
		/// </summary>
		private string _voiceRecognitionButtonText = BUTTON_TEXT_START;
		public string VoiceRecognitionButtonText
		{
			get { return _voiceRecognitionButtonText; }
			protected set { SetProperty(ref _voiceRecognitionButtonText, value); }
		}

		/// <summary>
		/// 音声認識を実行中かどうか（trueなら実行中）
		/// </summary>
		private bool _isRecognizing;
		public bool IsRecognizing
		{
			get { return _isRecognizing; }
			protected set
			{
				// 音声認識が実行中の場合、音声認識ボタンのテキストを「停止」に変更する。
				// 音声認識が停止している場合は「開始」に変更する。
				VoiceRecognitionButtonText = value ? BUTTON_TEXT_STOP : BUTTON_TEXT_START;
				SetProperty(ref _isRecognizing, value);
			}
		}

		/// <summary>
		/// 音声認識サービス
		/// </summary>
		private readonly IVoiceRecognitionService _voiceRecognitionService;

		/// <summary>
		/// 音声認識サービスの処理の呼び出し用コマンド
		/// </summary>
		public ICommand VoiceRecognitionCommand { get; }

		#endregion

		#region Constructor

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public MainPageViewModel(IVoiceRecognitionService voiceRecognitionService)
		{
			_voiceRecognitionService = voiceRecognitionService;

			// 音声認識サービスのプロパティが変更されたときに実行する処理を設定する。
			_voiceRecognitionService.PropertyChanged += voiceRecognitionServicePropertyChanged;

			// 音声認識サービスの処理本体をコマンドに紐付ける。
			VoiceRecognitionCommand = new DelegateCommand(executeVoiceRecognition);
		}

		#endregion

		#region Event of NavigationAware

		public void OnNavigatedFrom(NavigationParameters parameters)
		{

		}

		public void OnNavigatedTo(NavigationParameters parameters)
		{

		}

		public void OnNavigatingTo(NavigationParameters parameters)
		{
			
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// 音声認識サービスのプロパティ変更時にトリガーされるイベントの実処理
		/// </summary>
		/// <param name="sender">Sender</param>
		/// <param name="args">Arguments</param>
		private void voiceRecognitionServicePropertyChanged(object sender, PropertyChangedEventArgs args)
		{
			if (args.PropertyName == "RecognizedText")
			{
				// 音声の認識結果テキストの変更がトリガーになった場合、そのテキストをViewModelに取得する。
				RecognizedText = _voiceRecognitionService.RecognizedText;
			}
			if (args.PropertyName == "IsRecognizing")
			{
				// 音声認識の実行状況変更がトリガーになった場合、その実行状況をViewModelに取得する。
				IsRecognizing = _voiceRecognitionService.IsRecognizing;
			}
		}

		/// <summary>
		/// 音声認識サービス呼び出し用ボタンのコマンドの実処理
		/// </summary>
		private void executeVoiceRecognition()
		{
			if (IsRecognizing)
			{
				// 音声認識を実行中の場合、「停止」ボタンとして機能させる。
				_voiceRecognitionService.StopRecognizing();
			}
			else
			{
				// 音声認識が停止中の場合、「開始」ボタンとして機能させる。
				_voiceRecognitionService.StartRecognizing();
			}
		}

		#endregion
	}
}

