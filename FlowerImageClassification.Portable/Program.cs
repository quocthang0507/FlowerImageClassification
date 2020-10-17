using FlowerImageClassification.Shared;
using FlowerImageClassification.Shared.Common;
using System;
using System.IO;
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
		static void PrintMenu()
		{
			Console.OutputEncoding = Encoding.UTF8;
			Console.ForegroundColor = ConsoleColor.Cyan;
			Console.WriteLine(new String('=', 50));
			Console.WriteLine("0. Thoát khỏi chương trình");
			Console.WriteLine("1. Huấn luyện mô hình");
			Console.WriteLine("2. Đánh giá mô hình");
			Console.WriteLine("3. Dự đoán mô hình");
			Console.WriteLine(new String('=', 50));
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
			Console.WriteLine("\n" + new string('=', 50));
			Console.WriteLine("1. Chạy tự động với bộ thiết lập có sẵn");
			Console.WriteLine("    Chương trình sẽ tự động sử dụng các kiến trúc DNN được hỗ trợ bởi ML.NET:");
			Console.WriteLine("        ResnetV2101, InceptionV3, MobilenetV2, ResnetV250");
			Console.WriteLine("    Và tự động thay đổi kích thước tập huấn luyện và đánh giá:");
			Console.WriteLine("        0.5, 0.6, 0.7, 0.8, 0.9");
			Console.WriteLine("    Như vậy sẽ thực hiện tổng cộng 4 x 5 = 20 lần, thời gian chạy khá lâu, vui lòng không được tắt máy");
			Console.WriteLine("2. Tự chọn một kiến trúc và kích thước tập huấn luyện");
			Console.WriteLine(new string('=', 50));
			int function = SelectMenu(2);
			Console.Clear();
			Print_PathPrompt(out string outputModelPath, "Nhập đường dẫn đến thư mục sẽ lưu (các) mô hình đã được huấn luyện: ");
			Print_PathPrompt(out string fullImagesetFolderPath, "Nhập đường dẫn đến thư mục có chứa các thư mục tập hình ảnh: ");
			Print_PathPrompt(out string consoleOutputPath, "Nhập đường dẫn đến thư mục sẽ lưu (các) kết quả cửa sổ Console: ");
			string archName;
			string fileName;
			switch (function)
			{
				case 1:
					Console.Clear();
					float[] fractions = { 0.5f, 0.6f, 0.7f, 0.8f, 0.9f };
					foreach (Architecture _arch in (Architecture[])Enum.GetValues(typeof(Architecture)))
					{
						foreach (float _frac in fractions)
						{
							archName = Enum.GetName(typeof(Architecture), (int)_arch);
							fileName = $"{archName}_{_frac}";

							MirrorOutput _capturing = new MirrorOutput(Path.Combine(consoleOutputPath, fileName + ".txt"));
							Console.WriteLine($"==================== {archName} architecture, {_frac} ratio of train set with test set ====================");
							MLTraining _mlTraining = new MLTraining(Path.Combine(outputModelPath, fileName + ".zip"), null, fullImagesetFolderPath, null, _frac, (int)_arch);
							_mlTraining.RunPipeline();
							_capturing.Dispose();
						}
					}
					break;
				case 2:
					Architecture arch;
					float frac;
					while (true)
					{
						Console.Clear();
						Console.WriteLine("ML.NET hỗ trợ các kiến trúc DNN sau: ResnetV2101, InceptionV3, MobilenetV2, ResnetV250");
						Console.Write("Nhập tên kiến trúc cần sử dụng để huấn luyện mô hình: ");
						archName = Console.ReadLine();
						Console.Write("Nhập số thập phân kích thước tập huấn luyện so với tập đánh giá (0 < x < 1): ");
						string fracStr = Console.ReadLine();
						if (Enum.TryParse(archName, out arch) && float.TryParse(fracStr, out frac))
						{
							if (0f < frac && frac < 1f)
								break;
							Print_WarningText("Tỷ lệ phải lớn hơn 0 và nhỏ hơn 1");
						}
						Print_WarningText("Nội dung nhập không hợp lệ");
						Console.ReadKey();
					}
					archName = Enum.GetName(typeof(Architecture), (int)arch);
					fileName = $"{archName}_{frac}";
					MirrorOutput capturing = new MirrorOutput(Path.Combine(consoleOutputPath, fileName + ".txt"));
					Console.WriteLine($"==================== {archName} architecture, {frac} ratio of train set with test set ====================");
					MLTraining mlTraining = new MLTraining(Path.Combine(outputModelPath, fileName + ".zip"), null, fullImagesetFolderPath, null, frac, (int)arch);
					mlTraining.RunPipeline();
					capturing.Dispose();
					break;
				default:
					Print_WarningText("Nhập sai menu, vui lòng nhập lại!");
					break;
			}
		}

		static void PerformSubMenu_Evaluation()
		{

		}

		static void PerformSubMenu_Prediction()
		{

		}

		static void Print_PathPrompt(out string folderPath, string promptText)
		{
			while (true)
			{
				Console.Write(promptText);
				folderPath = Console.ReadLine();
				if (Directory.Exists(folderPath))
				{
					Print_SuccessText("Tìm thấy thư mục, bạn có thể tiếp tục");
					return;
				}
				else
				{
					Print_WarningText("Không tìm thấy thư mục, vui lòng nhập lại");
				}
			}
		}

		static void Print_WarningText(string text)
		{
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine(text);
			Console.ResetColor();
		}

		static void Print_SuccessText(string text)
		{
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine(text);
			Console.ResetColor();
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
						Console.WriteLine("Huấn luyện mô hình".ToUpper());
						PerformSubMenu_Training();
						break;
					case Menu.Evaluation:
						Console.WriteLine("Đánh giá mô hình".ToUpper());
						PerformSubMenu_Evaluation();
						break;
					case Menu.Prediction:
						Console.WriteLine("Dự đoán mô hình".ToUpper());
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
			Console.ReadLine();
		}
	}
}
