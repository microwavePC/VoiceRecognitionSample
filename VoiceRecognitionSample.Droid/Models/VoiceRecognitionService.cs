using System;
using Android.App;
using Android.Content;
using Android.Preferences;
using Android.Speech;
using Prism.Mvvm;
using VoiceRecognitionSample.Droid;
using VoiceRecognitionSample.Models.Droid;
using Xamarin.Forms;

[assembly: Dependency(typeof(VoiceRecognitionService))]
namespace VoiceRecognitionSample.Models.Droid
{
	/// <summary>
	/// 音声認識用サービス
	/// プロパティの変更をバインドで捉えられるようにするため、BindableBaseを継承する。
	/// </summary>
	public class VoiceRecognitionService : BindableBase, IVoiceRecognitionService
	{
		#region Properties

		/// <summary>
		/// 音声認識の実行状況（実行中の間のみtrueを返す）
		/// </summary>
		private bool _isRecognizing;
		public bool IsRecognizing
		{
			get { return _isRecognizing; }
			set { SetProperty(ref _isRecognizing, value); }
		}

		/// <summary>
		/// 音声認識の結果テキスト
		/// </summary>
		private string _recognizedText;
		public string RecognizedText
		{
			get
			{
				if (_recognizedText != null)
					return _recognizedText;
				else
					return string.Empty;
			}
			set { SetProperty(ref _recognizedText, value); }
		}

		#endregion

		#region Constant, MainActivity

		/// 定数・MainActivity
		private readonly int REQUEST_CODE_VOICE = 10;       // 音声認識のリクエストコード
		private readonly int INTERVAL_1500_MILLISEC = 1500; // 1.5秒（ミリ秒単位）
		private MainActivity mainActivity;                  // MainActivity

		#endregion

		#region Constructor

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public VoiceRecognitionService()
		{
			// 音声認識のアクティビティで取得した結果をハンドルする処理をMainActivityに付ける。
			mainActivity = Forms.Context as MainActivity;
			mainActivity.ActivityResult += HandleActivityResult;
		}

		#endregion

		#region Handler

		/// <summary>
		/// 音声認識のアクティビティで取得した結果をハンドルする処理の本体
		/// </summary>
		/// <param name="sender">Sender</param>
		/// <param name="args">Arguments</param>
		private void HandleActivityResult(object sender, PreferenceManager.ActivityResultEventArgs args)
		{
			if (args.RequestCode == REQUEST_CODE_VOICE)
			{
				IsRecognizing = false;
				if (args.ResultCode == Result.Ok)
				{
					// 認識が成功した場合、認識結果の文字列を引き出し、RecognizedTextに入れる。
					var matches = args.Data.GetStringArrayListExtra(RecognizerIntent.ExtraResults);
					if (matches.Count != 0)
					{
						RecognizedText = matches[0];
					}
				}
			}
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// 音声認識の開始処理
		/// </summary>
		public void StartRecognizing()
		{
			RecognizedText = string.Empty;
			IsRecognizing = true;

			try
			{
				// 音声認識のアクティビティを呼び出すためのインテントを用意する。
				var voiceIntent = new Intent(RecognizerIntent.ActionRecognizeSpeech);

				// 諸々のプロパティを設定する。
				voiceIntent.PutExtra(RecognizerIntent.ExtraLanguageModel, RecognizerIntent.LanguageModelFreeForm);
				voiceIntent.PutExtra(RecognizerIntent.ExtraPrompt, "聞き取った音声を画面上に表示します。");
				voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputCompleteSilenceLengthMillis, INTERVAL_1500_MILLISEC);
				voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputPossiblyCompleteSilenceLengthMillis, INTERVAL_1500_MILLISEC);
				voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputMinimumLengthMillis, INTERVAL_1500_MILLISEC);
				voiceIntent.PutExtra(RecognizerIntent.ExtraMaxResults, 1);

				// 認識言語の指定。端末の設定言語(Java.Util.Locale.Default)で音声認識を行う。
				voiceIntent.PutExtra(RecognizerIntent.ExtraLanguage, Java.Util.Locale.Default);

				// 音声認識のアクティビティを開始する。
				mainActivity.StartActivityForResult(voiceIntent, REQUEST_CODE_VOICE);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}

		/// <summary>
		/// 音声認識の停止処理
		/// </summary>
		public void StopRecognizing()
		{
			// Androidでは実装は不要。
		}

		#endregion
	}
}
