using System;
using OpenCvSharp;
using System.Drawing;
using System.Drawing.Imaging;

public class Fast : IModule
{
	private static Fast instance;
 
    private Fast() { }
 
    public static Fast GetInstance()
    {
        if (instance == null) instance = new Fast();
        return instance;
    }
    public void Cropp(Caretaker caretaker, string inputFile, string outputFile, string arg)
    {
        ProcessCropp(caretaker, inputFile, outputFile, arg);
    }
    public void RotateRight90(Caretaker caretaker, string inputFile, string outputFile)
    {
        ProcessRotate(caretaker, inputFile, outputFile);
    }
    public void Grayscale(Caretaker caretaker, string inputFile, string outputFile)
    {
        ProcessGrayscale(caretaker, inputFile, outputFile);
    }
    public void InvertColors(Caretaker caretaker, string inputFile, string outputFile)
    {
        ProcessInvertColors(caretaker, inputFile, outputFile);
    }
    public void ChangeHue(Caretaker caretaker, string inputFile, string outputFile, int hue)
    {
        ProcessChangeHue(caretaker, inputFile, outputFile, hue);
    }
    private void ProcessCropp(Caretaker caretaker, string inputFile, string outputFile, string rectFormat)
    {
        if (!rectFormat.Contains('x') && !rectFormat.Contains('+'))
        {
            Environment.ExitCode = 1;
            Console.Error.WriteLine("Wrong input paramethers.");
            return;
        }
        string[] parse = rectFormat.Split('x');
        if (!Int32.TryParse(parse[0], out int x))
        {
            Environment.ExitCode = 1;
            Console.Error.WriteLine("Wrong input paramethers.");
            return;
        }
        rectFormat = parse[1];
        parse = rectFormat.Split('+');
        if (!Int32.TryParse(parse[0], out int y))
        {
            Environment.ExitCode = 1;
            Console.Error.WriteLine("Wrong input paramethers.");
            return;
        }
        if (!Int32.TryParse(parse[1], out int w))
        {
            Environment.ExitCode = 1;
            Console.Error.WriteLine("Wrong input paramethers.");
            return;
        }
        if (!Int32.TryParse(parse[2], out int h))
        {
            Environment.ExitCode = 1;
            Console.Error.WriteLine("Wrong input paramethers.");
            return;
        }
        Mat bmp = new Mat(inputFile);
        if (x < 0 || y < 0 || w < 0 || h < 0)
        {
            Environment.ExitCode = 1;
            Console.Error.WriteLine($"Invalid paramethers. Paramethars must be bigger than 0.");
            return;
        }  
        Rect rect = new Rect(x, y, w, h);
        if (bmp.Cols < w + x || bmp.Rows < h + y)
        {
            Environment.ExitCode = 1;
            Console.Error.WriteLine($"Invalid paramethers. Input image is smaller.");
            return;
        }
        Mat result = new Mat(bmp, rect);
        Console.WriteLine($"Fast crop finished");
        //Bitmap bitimg = (Bitmap)result;
        caretaker.Backup(null);
        result.SaveImage(outputFile);
    }
    private void ProcessRotate(Caretaker caretaker, string inputFile, string outputFile)
    {
        Mat bmp = new Mat(inputFile);
        Mat result = new Mat();
        Point2f center = new Point2f(bmp.Height / 2, bmp.Height / 2);
        Mat matrix = Cv2.GetRotationMatrix2D(center, -90, 1);
        Cv2.WarpAffine(bmp, result, matrix, new OpenCvSharp.Size(bmp.Rows, bmp.Cols));
        Console.WriteLine($"Fast rotate finished");
        //Bitmap bitimg = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(result);
        caretaker.Backup(null);
        result.SaveImage(outputFile);
    }
    private void ProcessGrayscale(Caretaker caretaker, string inputFile, string outputFile)
    {
        Mat bmp = new Mat(inputFile);
        Mat result = new Mat();
        Mat[] channels = Cv2.Split(bmp); 
        result = channels[1];
        Console.WriteLine($"Fast grayscale finished");
        //Bitmap bitimg = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(result);
        caretaker.Backup(null);
        result.SaveImage(outputFile);
    }
    private void ProcessInvertColors(Caretaker caretaker, string inputFile, string outputFile)
    {
        Bitmap original = new Bitmap(inputFile);
        Bitmap newBitmap = new Bitmap(original.Width, original.Height);
        Graphics g = Graphics.FromImage(newBitmap);
        ColorMatrix colorMatrix = new ColorMatrix(
            new float[][]
            {
                new float[] {-1, 0, 0, 0, 0},
                new float[] {0, -1, 0, 0, 0},
                new float[] {0, 0, -1, 0, 0},
                new float[] {0, 0, 0, 1, 0},
                new float[] {1, 1, 1, 0, 1}
            });
        ImageAttributes attributes = new ImageAttributes();
        attributes.SetColorMatrix(colorMatrix);
        g.DrawImage(
            original,
            new Rectangle(0, 0, original.Width, original.Height),
            0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);
        attributes.Dispose();
        g.Dispose();
        newBitmap.Save(outputFile);
        caretaker.Backup(newBitmap);
        Console.WriteLine($"Fast invert colors finished");
    }
    private void ProcessChangeHue(Caretaker caretaker, string inputFile, string outputFile, int hue)
    {
        Bitmap original = new Bitmap(inputFile);
        Bitmap newBitmap = new Bitmap(original.Width, original.Height);
        Graphics g = Graphics.FromImage(newBitmap);
        ColorMatrix colorMatrix = new ColorMatrix(CreateHueMatrix(hue));
        ImageAttributes attributes = new ImageAttributes();
        attributes.SetColorMatrix(colorMatrix);
        g.DrawImage(
            original,
            new Rectangle(0, 0, original.Width, original.Height),
            0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);
        attributes.Dispose();
        g.Dispose();
        newBitmap.Save(outputFile);
        caretaker.Backup(newBitmap);
        Console.WriteLine($"Fast change hue finished.");
    }
    
    private float[][] CreateHueMatrix(float hueShiftDegrees)
    {
        float theta = (float)(hueShiftDegrees / 360 * 2 * Math.PI); 
        float c = (float)Math.Cos(theta);
        float s = (float)Math.Sin(theta);
        float A00 = 0.213f + 0.787f * c - 0.213f * s;
        float A01 = 0.213f - 0.213f * c + 0.413f * s;
        float A02 = 0.213f - 0.213f * c - 0.787f * s;
        float A10 = 0.715f - 0.715f * c - 0.715f * s;
        float A11 = 0.715f + 0.285f * c + 0.140f * s;
        float A12 = 0.715f - 0.715f * c + 0.715f * s;
        float A20 = 0.072f - 0.072f * c + 0.928f * s;
        float A21 = 0.072f - 0.072f * c - 0.283f * s;
        float A22 = 0.072f + 0.928f * c + 0.072f * s;
        return new float[][] {
            new float[]{A00, A01, A02, 0, 0},
            new float[]{A10, A11, A12, 0, 0},
            new float[]{A20, A21, A22, 0, 0},
            new float[]{0, 0, 0, 1, 0},
            new float[]{0, 0, 0, 0, 1}
        };
    }
}