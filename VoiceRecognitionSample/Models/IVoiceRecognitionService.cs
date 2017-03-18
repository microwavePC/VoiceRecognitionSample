using System.ComponentModel;

namespace VoiceRecognitionSample.Models
{
	/// 音声認識用のdependency service。
	/// プロパティの変更をViewModelで捕まえるため、INotifyPropertyChangedを継承している。
	public interface IVoiceRecognitionService : INotifyPropertyChanged
	{
		// 音声認識が実行中かどうか（実行中の間のみtrueを返す）
		bool IsRecognizing { get; }

		// 音声認識の結果テキスト（iOSの場合、認識結果をリアルタイムで取得できる）
		string RecognizedText { get; }

		// 音声認識の開始と停止
		void StartRecognizing();
		void StopRecognizing();
	}
}
