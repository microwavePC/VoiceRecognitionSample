using System.ComponentModel;

namespace VoiceRecognitionSample.Models
{
	/// <summary>
	/// 音声認識用サービス
	/// プロパティの変更をViewModelで捕まえるため、INotifyPropertyChangedを継承している。
	/// </summary>
	public interface IVoiceRecognitionService : INotifyPropertyChanged
	{
		/// <summary>
		/// 音声認識が実行中かどうか
		/// </summary>
		/// <value><c>true</c>実行中<c>false</c>停止中</value>
		bool IsRecognizing { get; }

		/// <summary>
		/// 音声認識の結果テキスト（iOSの場合、認識結果をリアルタイムで取得できる）
		/// </summary>
		/// <value>音声認識の結果テキスト</value>
		string RecognizedText { get; }

		/// <summary>
		/// 音声認識の開始処理
		/// </summary>
		void StartRecognizing();

		/// <summary>
		/// 音声認識の停止処理
		/// </summary>
		void StopRecognizing();
	}
}
