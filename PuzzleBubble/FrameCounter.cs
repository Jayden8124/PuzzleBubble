// using System.Collections.Generic;
// using System.Linq;

// namespace PuzzleBubblePJ{
//     public class FrameCount {
//         private static FrameCount instance;
//         public static FrameCount Instance {
//             get {
//                 if (instance == null) {
//                     instance = new FrameCount();
//                 }
//                 return instance;
//             }
//         }

//         public long TotalFrames { get; private set; }
//         public float TotalSeconds { get; private set; }
//         public float AverageFramesPerSecond { get; private set; }
//         public float CurrentFramesPerSecond { get; private set; }
//         public const int MaximumSamples = 100;

//         private Queue<float> _sampleBuffer = new Queue<float>();

//         public void update(float deltaTime) {
//             CurrentFramesPerSecond = 1.0f / deltaTime;

//             _sampleBuffer.Enqueue(CurrentFramesPerSecond);

//             if (_sampleBuffer.Count > MaximumSamples) {
//                 _sampleBuffer.Dequeue();
//                 AverageFramesPerSecond = _sampleBuffer.Average(i => i);
//             } else {
//                 AverageFramesPerSecond = CurrentFramesPerSecond;
//             }

//             TotalFrames++;
//             TotalSeconds += deltaTime;
//         }
//     }
// }


// // using System.Collections.Generic;
// // using System.Linq;
// // // https://stackoverflow.com/questions/20676185/xna-monogame-getting-the-frames-per-second
// // namespace GP_Midterm_BubblePuzzle {
// // 	public class FrameCounter {
// // 		private static FrameCounter instance;
// // 		public static FrameCounter Instance {
// // 			get {
// // 				if (instance == null)
// // 					instance = new FrameCounter();
// // 				return instance;
// // 			}
// // 		}
// // 		public long TotalFrames { get; private set; }
// // 		public float TotalSeconds { get; private set; }
// // 		public float AverageFramesPerSecond { get; private set; }
// // 		public float CurrentFramesPerSecond { get; private set; }

// // 		public const int MAXIMUM_SAMPLES = 100;

// // 		private Queue<float> _sampleBuffer = new Queue<float>();

// // 		public void Update(float deltaTime) {
// // 			CurrentFramesPerSecond = 1.0f / deltaTime;
// // 			_sampleBuffer.Enqueue(CurrentFramesPerSecond);
// // 			if (_sampleBuffer.Count > MAXIMUM_SAMPLES) {
// // 				_sampleBuffer.Dequeue();
// // 				AverageFramesPerSecond = _sampleBuffer.Average(i => i);
// // 			} else {
// // 				AverageFramesPerSecond = CurrentFramesPerSecond;
// // 			}
// // 			TotalFrames++;
// // 			TotalSeconds += deltaTime;
// // 		}
// // 	}
// // }