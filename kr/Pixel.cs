using System;
using System.Drawing;

public class Pixel : IModule
{
	private static Pixel instance;
 
    private Pixel() { }
 
    public static Pixel GetInstance()
    {
        if (instance == null) instance = new Pixel();
        return instance;
    }
    public void Cropp(Caretaker caretaker, string inputFile, string outputFile, string arg)
    {
        Bitmap input = new Bitmap(inputFile);
        ProcessCropp(caretaker, input, outputFile, arg);
    }
    public void RotateRight90(Caretaker caretaker, string inputFile, string outputFile)
    {
        Bitmap input = new Bitmap(inputFile);
        ProcessRotate(caretaker, input, outputFile);
    }
    public void Grayscale(Caretaker caretaker, string inputFile, string outputFile)
    {
        Bitmap input = new Bitmap(inputFile);
        ProcessGrayscale(caretaker, input, outputFile);
    }
    public void InvertColors(Caretaker caretaker, string inputFile, string outputFile)
    {
        Bitmap input = new Bitmap(inputFile);
        ProcessInvertColors(caretaker, input, outputFile);
    }
    public void ChangeHue(Caretaker caretaker, string inputFile, string outputFile, int hue)
    {
        Bitmap input = new Bitmap(inputFile);
        ProcessChangeHue(caretaker, input, outputFile, hue);
    }
    private static Rectangle RotateRect(Bitmap input)
    {
        return new Rectangle
        {
            Location = new System.Drawing.Point(0,0),
            Width = input.Height,
            Height = input.Width,
        };
    }
    private static Rectangle NormalRect(Bitmap input)
    {
        return new Rectangle
        {
            Location = new System.Drawing.Point(0,0),
            Width = input.Width,
            Height = input.Height,
        };
    }
    private static Rectangle ParseRect(string rectFormat)
    {
        if (!rectFormat.Contains('x') && !rectFormat.Contains('+'))
        {
            throw new ArgumentException("Wrong input paramethers.");
        }
        string[] parse = rectFormat.Split('x');
        if (!Int32.TryParse(parse[0], out int x))
            throw new ArgumentException("Wrong input paramethers.");
        rectFormat = parse[1];
        parse = rectFormat.Split('+');
        if (!Int32.TryParse(parse[0], out int y))
            throw new ArgumentException("Wrong input paramethers.");
        if (!Int32.TryParse(parse[1], out int w))
            throw new ArgumentException("Wrong input paramethers.");
        if (!Int32.TryParse(parse[2], out int h))
            throw new ArgumentException("Wrong input paramethers.");
        return new Rectangle
        {
            Location = new System.Drawing.Point(x, y),
            Width = w,
            Height = h,
        };
    }
    private static void ProcessCropp(Caretaker caretaker, Bitmap inputBitmap, string outputFile, string arg)
    {
        string cropArgs = arg;
        Rectangle cropRect = ParseRect(cropArgs); 
        Bitmap outputBitmap = Crop(inputBitmap, cropRect);
        Console.WriteLine($"Pixel crop finished");
        caretaker.Backup(outputBitmap);
        outputBitmap.Save(outputFile);
    }

    private static void ProcessRotate(Caretaker caretaker, Bitmap inputBitmap, string outputFile)
    {
        Rectangle rotateRect = RotateRect(inputBitmap); 
        Bitmap outputBitmap = Rotate(inputBitmap, rotateRect);
        Console.WriteLine($"Pixel rotate finished");
        caretaker.Backup(outputBitmap);
        outputBitmap.Save(outputFile);
    }
    private static void ProcessGrayscale(Caretaker caretaker, Bitmap inputBitmap, string outputFile)
    {
        Rectangle GrayscaleRect = NormalRect(inputBitmap); 
        Bitmap outputBitmap = RemoveC(inputBitmap, GrayscaleRect);
        Console.WriteLine($"Pixel grayscale finished");
        caretaker.Backup(outputBitmap);
        outputBitmap.Save(outputFile);
    }
    private static void ProcessInvertColors(Caretaker caretaker, Bitmap inputBitmap, string outputFile)
    {
        Rectangle invertRect = NormalRect(inputBitmap); 
        Bitmap outputBitmap = InvertC(inputBitmap, invertRect);
        Console.WriteLine($"Pixel invert colors finished");
        caretaker.Backup(outputBitmap);
        outputBitmap.Save(outputFile);
    }
    private static void ProcessChangeHue(Caretaker caretaker, Bitmap inputBitmap, string outputFile, int hue)
    {
        Rectangle changeHueRect = NormalRect(inputBitmap); 
        Bitmap outputBitmap = ChangeH(inputBitmap, changeHueRect, hue);
        Console.WriteLine($"Pixel change hue finished");
        caretaker.Backup(outputBitmap);
        outputBitmap.Save(outputFile);
    }
    private static void ValidateCropRect(Bitmap bmp, Rectangle rect)
    {
        if (rect.Left <= 0 || rect.Left >= bmp.Width)
        {
            throw new ArgumentException($"Invalid left: {rect.Left}");
        }
        if (rect.Right >= bmp.Width)
        {
            throw new ArgumentException($"Invalid right: {rect.Right}");
        }
        if (rect.Top <= 0 || rect.Top >= bmp.Height)
        {
            throw new ArgumentException($"Invalid top: {rect.Top}");
        }
        if (rect.Bottom >= bmp.Height)
        {
            throw new ArgumentException($"Invalid right: {rect.Bottom}");
        }
    }
    private static Bitmap Crop(Bitmap bmp, Rectangle rect)
    {
        Bitmap croped = new Bitmap(rect.Width, rect.Height);
        ValidateCropRect(bmp, rect);
        for (int y = 0; y < croped.Height; y++)
        {
            for(int x = 0; x < croped.Width; x++)
            {
                Color color = bmp.GetPixel(x + rect.Left, y + rect.Top);
                croped.SetPixel(x, y, color);
            }
        }
        return croped;
    }
    private static Bitmap Rotate(Bitmap bmp, Rectangle rect)
    {
        Bitmap rotated = new Bitmap(rect.Width, rect.Height);
        for (int y = 0; y < rotated.Width; y++)
        {
            for(int x = 0; x < rotated.Height; x++)
            {
                Color color = bmp.GetPixel(x, y);
                rotated.SetPixel(rect.Width - 1 - y, x, color);
            }
        }
        return rotated;
    }
    private static Bitmap RemoveC(Bitmap bmp, Rectangle rect)
    {
        Bitmap grayscale = new Bitmap(rect.Width, rect.Height);
        for (int y = 0; y < grayscale.Height; y++)
        {
            for(int x = 0; x < grayscale.Width; x++)
            {
                Color color = bmp.GetPixel(x, y);
                int yLinear = (int)(0.2126 * color.R + 0.7152 * color.G + 0.0722 * color.B);
                Color newColor = Color.FromArgb(255, yLinear, yLinear, yLinear);;
                grayscale.SetPixel(x, y, newColor);
            }
        }
        return grayscale;
    }
    private static Bitmap InvertC(Bitmap bmp, Rectangle rect)
    {
        Bitmap invertColors = new Bitmap(rect.Width, rect.Height);
        for (int y = 0; y < invertColors.Height; y++)
        {
            for(int x = 0; x < invertColors.Width; x++)
            {
                Color color = bmp.GetPixel(x, y);
                Color newColor = Color.FromArgb(255, 255 - color.R, 255 - color.G, 255 - color.B);
                invertColors.SetPixel(x, y, newColor);
            }
        }
        return invertColors;
    }
    private static Bitmap ChangeH(Bitmap bmp, Rectangle rect, int hue)
    {
        Bitmap changeHueColors = new Bitmap(rect.Width, rect.Height);
        for (int y = 0; y < changeHueColors.Height; y++)
        {
            for(int x = 0; x < changeHueColors.Width; x++)
            {
                Color color = bmp.GetPixel(x, y);
                int R = color.R;
                int G = color.G;
                int B = color.B;
                double h = 180;
                double l = 0;
                double s = 0;
                RgbToHls(R, G, B, out h, out l, out s);
                h += hue;
                HlsToRgb(h, l, s, out R, out G, out B);
                Color newColor = Color.FromArgb(255, R, G, B);
                changeHueColors.SetPixel(x, y, newColor);
            }
        }
        return changeHueColors;
    }
    private static void RgbToHls(int r, int g, int b, out double h, out double l, out double s)
    {
        // Convert RGB to a 0.0 to 1.0 range.
        double double_r = r / 255.0;
        double double_g = g / 255.0;
        double double_b = b / 255.0;
    
        // Get the maximum and minimum RGB components.
        double max = double_r;
        if (max < double_g) max = double_g;
        if (max < double_b) max = double_b;
    
        double min = double_r;
        if (min > double_g) min = double_g;
        if (min > double_b) min = double_b;
    
        double diff = max - min;
        l = (max + min) / 2;
        if (Math.Abs(diff) < 0.00001)
        {
            s = 0;
            h = 0;  // H is really undefined.
        }
        else
        {
            if (l <= 0.5) s = diff / (max + min);
            else s = diff / (2 - max - min);
    
            double r_dist = (max - double_r) / diff;
            double g_dist = (max - double_g) / diff;
            double b_dist = (max - double_b) / diff;
    
            if (double_r == max) h = b_dist - g_dist;
            else if (double_g == max) h = 2 + r_dist - b_dist;
            else h = 4 + g_dist - r_dist;
    
            h = h * 60;
            if (h < 0) h += 360;
        }
    }
    private static void HlsToRgb(double h, double l, double s, out int r, out int g, out int b)
    {
        double p2;
        if (l <= 0.5) p2 = l * (1 + s);
        else p2 = l + s - l * s;
    
        double p1 = 2 * l - p2;
        double double_r, double_g, double_b;
        if (s == 0)
        {
            double_r = l;
            double_g = l;
            double_b = l;
        }
        else
        {
            double_r = QqhToRgb(p1, p2, h + 120);
            double_g = QqhToRgb(p1, p2, h);
            double_b = QqhToRgb(p1, p2, h - 120);
        }
    
        // Convert RGB to the 0 to 255 range.
        r = (int)(double_r * 255.0);
        g = (int)(double_g * 255.0);
        b = (int)(double_b * 255.0);
    }
    private static double QqhToRgb(double q1, double q2, double hue)
    {
        if (hue > 360) hue -= 360;
        else if (hue < 0) hue += 360;
    
        if (hue < 60) return q1 + (q2 - q1) * hue / 60;
        if (hue < 180) return q2;
        if (hue < 240) return q1 + (q2 - q1) * (240 - hue) / 60;
        return q1;
    }
}

