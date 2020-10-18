using FlowerImageClassification.Shared;
using FlowerImageClassification.Shared.Common;
using FlowerImageClassification.Shared.ImageSchema;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace FlowerImageClassification.Portable
{
	public enum Menu
	{
		Exit,
		Training,
		Evaluation,
		Prediction
	}

	class Program
	{
		static float[] trainingFractions = { 0.5f, 0.6f, 0.7f, 0.8f, 0.9f };
		static float[] evaluationFractions = { 0.1f, 0.2f, 0.3f, 0.4f, 0.5f };

		static void PrintMenu()
		{
			Console.OutputEncoding = Encoding.UTF8;
			Console.ForegroundColor = ConsoleColor.Cyan;
			WriteHelper.Print_CenteredTitle("VUI LÒNG CHỌN CHỨC NĂNG DƯỚI ĐÂY:", 100);
			Console.WriteLine(new String('=', 100));
			Console.WriteLine("0. Thoát khỏi chương trình");
			Console.WriteLine("1. Huấn luyện mô hình");
			Console.WriteLine("2. Đánh giá mô hình");
			Console.WriteLine("3. Dự đoán mô hình");
			Console.WriteLine(new String('=', 100));
			Console.ResetColor();
		}

		static int SelectMenu(int total)
		{
			int id;
			while (true)
			{
				Console.Write("Nhập chức năng cần thực hiện: ");
				bool ok = int.TryParse(Console.ReadLine(), out id);
				if (ok && 0 <= id && id <= total)
					break;
			}
			return id;
		}

		static void PerformSubMenu_Training()
		{
			Console.ForegroundColor = ConsoleColor.Cyan;
			WriteHelper.Print_CenteredTitle("VUI LÒNG CHỌN CHỨC NĂNG DƯỚI ĐÂY:", 100);
			Console.WriteLine("\n" + new string('=', 100));
			Console.WriteLine("1. Chạy tự động với bộ thiết lập có sẵn");
			Console.WriteLine("    Chương trình sẽ tự động sử dụng các kiến trúc DNN được hỗ trợ bởi ML.NET:");
			Console.WriteLine("        ResnetV2101, InceptionV3, MobilenetV2, ResnetV2100");
			Console.WriteLine("    Và tự động thay đổi kích thước tập huấn luyện:");
			Console.WriteLine("        {0.5, 0.6, 0.7, 0.8, 0.9}");
			Console.WriteLine("    Như vậy sẽ thực hiện tổng cộng 4 x 5 = 20 lần, thời gian chạy khá lâu, vui lòng không được tắt trong khi chạy");
			Console.WriteLine("2. Tự chọn một kiến trúc và kích thước tập huấn luyện");
			Console.WriteLine(new string('=', 100));
			Console.ResetColor();
			int function = SelectMenu(2);
			Console.Clear();
			Print_FolderPathPrompt(out string outputModelPath, "Nhập đường dẫn đến thư mục sẽ lưu (các) mô hình đã được huấn luyện: ");
			Print_FolderPathPrompt(out string fullImagesetFolderPath, "Nhập đường dẫn đến thư mục có chứa các thư mục tập hình ảnh: ");
			Print_FolderPathPrompt(out string consoleOutputPath, "Nhập đường dẫn đến thư mục sẽ lưu (các) kết quả cửa sổ Console: ");
			string archNameInput, modelFileName;
			switch (function)
			{
				case 1:
					Console.Clear();
					foreach (Architecture _arch in (Architecture[])Enum.GetValues(typeof(Architecture)))
					{
						foreach (float f in trainingFractions)
						{
							archNameInput = Enum.GetName(typeof(Architecture), (int)_arch);
							modelFileName = $"{archNameInput}_{f}_{DateTime.Now.ToString("HH-mm-ss")}";

							MirrorOutput capturing_1 = new MirrorOutput(Path.Combine(consoleOutputPath, modelFileName + ".txt"));
							Console.WriteLine($"==================== {archNameInput} architecture, {f} ratio of train set with test set ====================");
							MLTraining mlTraining_1 = new MLTraining(Path.Combine(outputModelPath, modelFileName + ".zip"), null, fullImagesetFolderPath, null, f, (int)_arch);
							mlTraining_1.RunPipeline();
							capturing_1.Dispose();
						}
					}
					break;
				case 2:
					Architecture arch;
					float frac;
					while (true)
					{
						Console.Clear();
						Console.WriteLine("ML.NET hỗ trợ các kiến trúc DNN sau: ResnetV2101, InceptionV3, MobilenetV2, ResnetV2100");
						Console.Write("Nhập tên kiến trúc cần sử dụng để huấn luyện mô hình: ");
						archNameInput = Console.ReadLine();
						Console.Write("Nhập số thập phân kích thước tập huấn luyện so với tập đánh giá (0 < x < 1): ");
						string fracInput = Console.ReadLine();
						if (Enum.TryParse(archNameInput, out arch) && float.TryParse(fracInput, out frac))
						{
							if (0f < frac && frac < 1f)
								break;
							WriteHelper.Print_WarningText("Tỷ lệ phải lớn hơn 0 và nhỏ hơn 1");
						}
						WriteHelper.Print_WarningText("Nội dung nhập không hợp lệ");
						Console.ReadKey();
					}
					archNameInput = Enum.GetName(typeof(Architecture), (int)arch);
					modelFileName = $"{archNameInput}_{frac}_{DateTime.Now.ToString("HH-mm-ss")}";
					MirrorOutput capturing_2 = new MirrorOutput(Path.Combine(consoleOutputPath, modelFileName + ".txt"));
					Console.WriteLine($"==================== {archNameInput} architecture, {frac} ratio of train set with test set ====================");
					MLTraining mlTraining_2 = new MLTraining(Path.Combine(outputModelPath, modelFileName + ".zip"), null, fullImagesetFolderPath, null, frac, (int)arch);
					mlTraining_2.RunPipeline();
					capturing_2.Dispose();
					break;
				default:
					WriteHelper.Print_WarningText("Nhập sai menu, vui lòng nhập lại!");
					break;
			}
			Console.WriteLine("Nhấn phím bất kỳ để quay trở lại...");
		}

		static void PerformSubMenu_Evaluation()
		{
			Console.ForegroundColor = ConsoleColor.Cyan;
			WriteHelper.Print_CenteredTitle("VUI LÒNG CHỌN CHỨC NĂNG DƯỚI ĐÂY:", 100);
			Console.WriteLine("\n" + new string('=', 100));
			Console.WriteLine("1. Đánh giá tất cả các mô hình đã được huấn luyện");
			Console.WriteLine("    Chương trình sẽ tự động tìm và sử dụng các mô hình đã huấn luyện thông qua định dạng tập tin *.zip:");
			Console.WriteLine("    Và tự động thay đổi kích thước tập đánh giá:");
			Console.WriteLine("        {0.1, 0.2, 0.3, 0.4, 0.5}");
			Console.WriteLine("    Như vậy sẽ thực hiện (số mô hình tìm thấy) x 5 lần, thời gian chạy khá lâu, vui lòng không được tắt trong khi chạy");
			Console.WriteLine("2. Tự chọn một kiến trúc và kích thước tập đánh giá");
			WriteHelper.Print_WarningText("Lưu ý: Hình ảnh đã được học nếu đem đi đánh giá sẽ không khách quan, do đó phải sử dụng \nkích thước tập đánh giá = 1 - kích thước tập huấn luyện");
			Console.WriteLine(new string('=', 100));
			Console.ResetColor();
			int function = SelectMenu(2);
			Console.Clear();
			Print_FolderPathPrompt(out string outputModelPath, "Nhập đường dẫn đến thư mục đã lưu (các) mô hình đã được huấn luyện: ");
			Print_FolderPathPrompt(out string fullImagesetFolderPath, "Nhập đường dẫn đến thư mục có chứa các thư mục tập hình ảnh: ");
			Print_FolderPathPrompt(out string consoleOutputPath, "Nhập đường dẫn đến thư mục sẽ lưu (các) kết quả cửa sổ Console: ");
			string consoleFileName;
			switch (function)
			{
				case 1:
					Console.Clear();
					var foundTrainedModels = Directory.GetFiles(outputModelPath, "*", SearchOption.TopDirectoryOnly).
						Where(file => Path.GetExtension(file).Contains("zip", StringComparison.OrdinalIgnoreCase));
					if (foundTrainedModels.Count() == 0)
					{
						WriteHelper.Print_WarningText("Không tìm thấy bất kỳ tập tin *.zip nào trong thư mục này cả");
						return;
					}
					foreach (var modelPath in foundTrainedModels)
						foreach (var f in evaluationFractions)
						{
							consoleFileName = $"EvaluationResult_{Path.GetFileName(modelPath)}_{DateTime.Now.ToString("HH-mm-ss")}";
							MirrorOutput capturing_1 = new MirrorOutput(Path.Combine(consoleOutputPath, consoleFileName + ".txt"));
							Console.WriteLine($"==================== {Path.GetFileName(modelPath)} architecture, {f} ratio of test set with train set====================");
							MLTraining mlTraining_1 = new MLTraining(modelPath, null, fullImagesetFolderPath, null, f);
							mlTraining_1.EvaluateModel();
							capturing_1.Dispose();
						}
					break;
				case 2:
					float frac;
					string fileName;
					while (true)
					{
						Console.Clear();
						Print_FilePathPrompt(out fileName, "Nhập đường dẫn đến mô hình đã được huấn luyện trước: ");
						Console.Write("Nhập số thập phân kích thước tập đánh giá so với tập huấn luyện (= 1 - kích thước tập huấn luyện): ");
						string fracStr = Console.ReadLine();
						if (float.TryParse(fracStr, out frac))
						{
							if (0f < frac && frac < 1f)
								break;
							WriteHelper.Print_WarningText("Tỷ lệ phải lớn hơn 0 và nhỏ hơn 1");
						}
						WriteHelper.Print_WarningText("Nội dung nhập không hợp lệ");
						Console.ReadKey();
					}
					consoleFileName = $"EvaluationResult_{fileName}_{DateTime.Now.ToString("HH-mm-ss")}";
					MirrorOutput capturing_2 = new MirrorOutput(Path.Combine(consoleOutputPath, consoleFileName + ".txt"));
					Console.WriteLine($"==================== {fileName} architecture, {frac} ratio of test set with train set====================");
					MLTraining mlTraining_2 = new MLTraining(consoleFileName, null, fullImagesetFolderPath, null, 1 - frac);
					mlTraining_2.EvaluateModel();
					capturing_2.Dispose();
					break;
				default:
					WriteHelper.Print_WarningText("Nhập sai menu, vui lòng nhập lại!");
					break;
			}
			Console.WriteLine("Nhấn phím bất kỳ để quay trở lại...");
		}

		static void PerformSubMenu_Prediction()
		{
			Console.ForegroundColor = ConsoleColor.Cyan;
			WriteHelper.Print_CenteredTitle("VUI LÒNG CHỌN CHỨC NĂNG DƯỚI ĐÂY:", 100);
			Console.WriteLine("\n" + new string('=', 100));
			Console.WriteLine("1. Tự động dự đoán tất cả các bức hình có trong thư mục ");
			Console.WriteLine("2. Dự đoán một bức hình tùy ý");
			WriteHelper.Print_WarningText("Lưu ý: Chỉ nhận hình ảnh là tập tin *.jpg hoặc *.png");
			Console.WriteLine(new string('=', 100));
			Console.ResetColor();
			int function = SelectMenu(2);
			string trainedModelPath;
			switch (function)
			{
				case 1:
					Console.Clear();
					Print_FilePathPrompt(out trainedModelPath, "Nhập đường dẫn đến mô hình đã được huấn luyện trước: ");
					Print_FolderPathPrompt(out string predictionFolderPath, "Nhập đường dẫn đến thư mục hình ảnh cần dự đoán: ");
					MLTraining mlTraining_1 = new MLTraining(trainedModelPath, predictionFolderPath, null);
					mlTraining_1.TryMultiplePredictions();
					break;
				case 2:
					Console.Clear();
					Print_FilePathPrompt(out trainedModelPath, "Nhập đường dẫn đến mô hình đã được huấn luyện trước: ");
					ConsumeModel.MLNetModelPath = trainedModelPath;
					while (true)
					{
						if (Print_FilePathPrompt(out string imagePath, "Nhập đường dẫn hình ảnh cần dự đoán, hoặc gõ 'exit' để THOÁT: ") == 0)
							break;
						ImageDataInMemory imageData = new ImageDataInMemory(imagePath, Path.GetFileName(imagePath));
						var predictionResult = ConsumeModel.Predict(imageData);
						Console.WriteLine($"Tên dự đoán: {predictionResult.PredictedLabel}");
						Console.WriteLine($"Độ chính xác: {predictionResult.Score.Max()}");
					}
					break;
				default:
					break;
			}
			Console.WriteLine("Nhấn phím bất kỳ để quay trở lại...");
		}

		static void Print_FolderPathPrompt(out string folderPath, string promptText)
		{
			while (true)
			{
				Console.Write(promptText);
				Console.BackgroundColor = ConsoleColor.White;
				Console.ForegroundColor = ConsoleColor.Black;
				folderPath = Console.ReadLine();
				Console.ResetColor();
				folderPath = folderPath.Replace("\"", "");
				if (Directory.Exists(folderPath))
				{
					WriteHelper.Print_SuccessText("Tìm thấy thư mục, bạn có thể tiếp tục");
					return;
				}
				else
				{
					WriteHelper.Print_WarningText("Không tìm thấy thư mục, vui lòng nhập lại");
				}
			}
		}

		/// <summary>
		/// Return 0 if user input equals 'exit', 1 if user input is a valid file path
		/// </summary>
		/// <param name="filePath"></param>
		/// <param name="promptText"></param>
		/// <returns></returns>
		static int Print_FilePathPrompt(out string filePath, string promptText)
		{
			while (true)
			{
				Console.Write(promptText);
				Console.BackgroundColor = ConsoleColor.White;
				Console.ForegroundColor = ConsoleColor.Black;
				filePath = Console.ReadLine();
				Console.ResetColor();
				filePath = filePath.Replace("\"", "");
				if (filePath.Equals("exit", StringComparison.OrdinalIgnoreCase))
					return 0;
				if (File.Exists(filePath))
				{
					WriteHelper.Print_SuccessText("Tìm thấy tập tin, bạn có thể tiếp tục");
				}
				else
				{
					WriteHelper.Print_WarningText("Không tìm thấy tập tin, vui lòng nhập lại");
				}
				return 1;
			}
		}



		static void PerformMenu()
		{
			while (true)
			{
				Console.Clear();
				PrintMenu();
				Menu function = (Menu)SelectMenu(Enum.GetNames(typeof(Menu)).Length);
				Console.Clear();
				switch (function)
				{
					case Menu.Exit:
						Console.WriteLine("Thoát khỏi chương trình, nhấn phím bất kỳ để thoát...");
						return;
					case Menu.Training:
						PerformSubMenu_Training();
						break;
					case Menu.Evaluation:
						PerformSubMenu_Evaluation();
						break;
					case Menu.Prediction:
						PerformSubMenu_Prediction();
						break;
					default:
						Console.WriteLine("Nhập sai menu, vui lòng nhập lại!");
						break;
				}
				Console.ReadKey();
			}
		}

		static void Main(string[] args)
		{
			PerformMenu();
			Console.WriteLine("Nhấn phím bất kỳ để thoát...");
			Console.ReadLine();
		}
	}
}
