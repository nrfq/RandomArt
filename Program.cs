using System;
using System.Reflection;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Random_Art
{   
    // A class containing helper functions for various expressions
    public static class Help{
        public static float tri(float x){
            return 1 - 2 * Math.Abs(x);
        }
        public static float gravity(float x){
            return 1 - 2 / (float)Math.Pow(1 + x*x, 8);
        }
    }
    public interface Expression{
        static int Arity{ get;}
        string toString();
        float[] eval(float x,  float y);
    }

    public class XCoor : Expression{
        public static int Arity {get;}
        static XCoor(){
            Arity = 0;
        }
        public XCoor(){}
        public string toString(){
            return "XCoor()";
        }
        public float[] eval(float x, float y){
            float[] rgb = {x, x, x};
            return rgb;
        }
    }

    public class YCoor : Expression{
        public static int Arity{get;}
        static YCoor(){
            Arity = 0;
        }
        public YCoor(){}
        public string toString(){
            return "YCoor()";
        }
        public float[] eval(float x, float y){
            float[] rgb = {y, y, y};
            return rgb;
        }
    }
    public class RandPoints : Expression{
        public static int Arity{get;}
        float[] rgb = new float[3];
        static RandPoints(){
            Arity = 0;
        }
        public RandPoints(){
            Random rand = new Random();
            rgb[0] = (float)rand.NextDouble();
            rgb[1] = (float)rand.NextDouble();
            rgb[2] = (float)rand.NextDouble();
        }
        public string toString(){
            return "RandPoints()";
        }
        public float[] eval(float x, float y){
            return rgb;
        }
    }
    public class Product : Expression{
        Expression Exp1;
        Expression Exp2;
        public static int Arity {get;}

        static Product(){
            Arity = 2;
        }
        public Product(){
            Exp1 = new XCoor();
            Exp2 = new YCoor();
        }
        public Product(Expression exp1, Expression exp2){
            Exp1 = exp1;
            Exp2 = exp2;
        }
        public string toString(){
            return "Product(" + Exp1.toString() + ", " + Exp2.toString() + ")";
        }
        public float[] eval(float x, float y){
            float[] rgb1 = Exp1.eval(x, y);
            float[] rgb2 = Exp2.eval(x, y);
            float[] rgb3 = { rgb1[0] * rgb2[0], rgb1[1] * rgb2[1], rgb1[2] * rgb2[2]};
            return rgb3;
        }
    }
    public class Sin : Expression{
        Expression Exp1;
        float phase;
        float frequency;
        public static int Arity {get;}

        static Sin(){
            Arity = 1;
        }
        public Sin(){
            Exp1 = new XCoor();
        }
        public Sin(Expression exp1){
            Exp1 = exp1;
            Random rand = new Random();
            phase = (float)(rand.NextDouble() * Math.PI);
            frequency = (float)( (rand.NextDouble() * 5.0 + 1.0));
        }
        public string toString(){
            return "Sin(" + Exp1.toString() + ")";
        }

        public float[] eval(float x, float y){
            float[] rgb1 = Exp1.eval(x, y);
            float[] rgb2 = {(float)Math.Sin(rgb1[0] * frequency + phase), (float)Math.Sin(rgb1[1] * frequency + phase), (float)Math.Sin(rgb1[2] * frequency + phase)};
            return rgb2;
        }
    }
    public class Avg : Expression{
        Expression Exp1;
        Expression Exp2;
        public static int Arity{get;}
        static Avg(){
            Arity = 2;
        }
        public Avg(){
            Exp1 = new XCoor();
            Exp2 = new YCoor();
        }
        public Avg(Expression exp1, Expression exp2){
            Exp1 = exp1;
            Exp2 = exp2;
        }
        public string toString(){
            return "Avg(" + Exp1.toString() + ", " + Exp2.toString() + ")";
        }
        public float[] eval(float x, float y){
            float[] rgb1 = Exp1.eval(x, y);
            float[] rgb2 = Exp2.eval(x, y);
            float[] rgb3 = { (rgb1[0] + rgb2[0])/2, (rgb1[1] + rgb2[1])/2, (rgb1[2]+rgb2[2])/2}; 
            return rgb3;
        }
    }
    public class Gravity : Expression{
        Expression Exp1;
        public static int Arity{get;}
        static Gravity(){
            Arity = 1;
        }
        public Gravity(){
            Exp1 = new XCoor();
        }
        public Gravity(Expression exp1){
            Exp1 = exp1;
        }
        public string toString(){
            return "Gravity(" + Exp1.toString() + ")";
        }
        public float[] eval(float x, float y){
            float[] rgb1 = Exp1.eval(x, y);
            float[] rgbr = { Help.gravity(rgb1[0]), Help.gravity(rgb1[1]), Help.gravity(rgb1[2]) };
            return rgbr;
        }
    }
    public class WeightedAverage : Expression{
        Expression Exp1;
        Expression Exp2;
        Expression Weight;
        public static int Arity{get;}
        static WeightedAverage(){
            Arity = 3;
        }
        public WeightedAverage(){
            Exp1 = new XCoor();
            Exp2 = new YCoor();
            Weight = new XCoor();
        }
        public WeightedAverage(Expression exp1, Expression exp2, Expression weight){
            Exp1 = exp1;
            Exp2 = exp1;
            Weight = weight;
        }
        public string toString(){
            return "WeightedAverage(" + Exp1.toString() + ", " + Exp2.toString() + ", weight)";
        }
        public float[] eval(float x, float y){
            float[] rgb1 = Exp1.eval(x, y);
            float[] rgb2 = Exp2.eval(x, y);
            float w = (float)( (Weight.eval(x, y)[0] + 1.0) / 2);
            float[] rgb3 = { w*rgb1[0] + (1-w)*rgb2[0], w*rgb1[1] + (1-w)*rgb2[1], w*rgb1[2] + (1-w)*rgb2[2] };
            return rgb3;
        }
    }
    public class Tri : Expression{
        Expression Exp1;
        public static int Arity{get;}
        static Tri(){
            Arity = 1;
        }
        public Tri(){
            Exp1 = new XCoor();
        }
        public Tri(Expression exp1){
            Exp1 = exp1;
        }
        public string toString(){
            return "Tri(" + Exp1.toString() + ")";
        }
        public float[] eval(float x, float y){
            float[] rgb1 = Exp1.eval(x, y);
            float[] rgbr = { Help.tri(rgb1[0]), Help.tri(rgb1[1]), Help.tri(rgb1[2])};
            return rgbr;
        }
    }
    //Has an error where everything becomes a black cube, fix later
    /*public class Mod : Expression{
        Expression Exp1;
        Expression Exp2;
        public static int Arity{get;}
        static Mod(){
            Arity = 2;
        }
        public Mod(){
            Exp1 = new XCoor();
            Exp2 = new YCoor();
        }
        public Mod(Expression exp1, Expression exp2){
            Exp1 = exp1;
            Exp2 = exp2;
        }
        public string toString(){
            return "Mod(" + Exp1.toString() + ", " + Exp2.toString() + ")";
        }
        public float[] eval(float x, float y){
            try{
                float[] rgb1 = Exp1.eval(x,y);
                float[] rgb2 = Exp2.eval(x,y);
                float[] rgbr = { rgb1[0] % rgb2[0], rgb1[1] % rgb2[1], rgb1[2] % rgb2[2] };
                return rgbr;
            }
            catch{
                float[] rgbr = {0, 0, 0};
                return rgbr;
            }
        }
    }*/

    /*public class Level : Expression{
        Expression Exp1;
        Expression Exp2;
        Expression level;
        float threshold;
        public static int Arity{get;}
        static Level(){
            Arity = 3;
        }
        public Level(Expression exp1, Expression exp2, Expression exp3){
            Random rand = new Random();
            threshold = (float)(rand.NextDouble()*2 - 1);
            Exp1 = exp1;
            Exp2 = exp2;
            level = exp3;
        }
        public string toString(){
            return "Level(" + Exp1.toString() + ", " + Exp2.toString() + ", " + level.toString() + ", threshold: " + threshold + ")";
        }
        public float[] eval(float x, float y){
            float[] rgb1 = Exp1.eval(x, y);
            float[] rgb2 = Exp2.eval(x, y);
            float[] rgb3 = level.eval(x,y);
            float[] rgbr = new float[3];
            if(rgb1[0] < threshold){rgbr[0] = rgb2[0];} else{rgbr[0] = rgb3[0];}
            if(rgb1[1] < threshold){rgbr[1] = rgb2[1];} else{rgbr[1] = rgb3[1];}
            if(rgb1[2] < threshold){rgbr[2] = rgb2[2];} else{rgbr[2] = rgb3[2];}
            return rgbr;
        }
    }*/

    public class Program
    {   
        //GetPixel SetPixel implementation of grayscale conversion
        public void iterativeGrayscale(Bitmap bmp){
            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    Color c = bmp.GetPixel(i, j);

                    //Apply conversion equation
                    byte gray = (byte)(.21 * c.R + .71 * c.G + .071 * c.B);

                    //Set the color of this pixel
                    bmp.SetPixel(i, j, Color.FromArgb(gray, gray, gray));
                }
            }
            bmp.Save("output.jpg", ImageFormat.Jpeg);
        }

        public void lockedGrayscale(Bitmap bmp){
            //Lock bitmap's bits to system memory
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, bmp.PixelFormat);

            //Scan for the first line
            IntPtr ptr = bmpData.Scan0;

            //Declare an array in which your RGB values will be stored
            int bytes = Math.Abs(bmpData.Stride) * bmp.Height;
            byte[] rgbValues = new byte[bytes];

            //Copy RGB values in that array
            Marshal.Copy(ptr, rgbValues, 0, bytes);

            for (int i = 0; i < rgbValues.Length; i += 4)
            {
                //Set RGB values in a Array where all RGB values are stored
                //byte gray = (byte)(rgbValues[i] * 0.21 + rgbValues[i + 1] * 0.71 + rgbValues[i + 2] * 0.071);
                byte gray = (byte)( (rgbValues[i] + rgbValues[i+1] + rgbValues[i+2]) / 3);
                rgbValues[i] = rgbValues[i + 1] = rgbValues[i + 2] = gray;
            }

            //Copy changed RGB values back to bitmap
            Marshal.Copy(rgbValues, 0, ptr, bytes);

            //Unlock the bits
            bmp.UnlockBits(bmpData);

            bmp.Save("output.png", ImageFormat.Png);
        }

        public static Expression createExpression(List<Type> expressionTypes0, List<Type> expressionTypes1, int level = 10){
            Random random = new Random();
            if(level <= 0){
                //Reached a leaf of the expression tree, generate expression with 0 arity
                int rand = random.Next(expressionTypes0.Count);
                return (Expression)Activator.CreateInstance(expressionTypes0[rand]);
            }
            else{
                //Internal node of expression tree, generate expression with arity > 1
                int rand = random.Next(expressionTypes1.Count);
                Type toCreate = expressionTypes1[rand];
                //gets the arity of the chosen type
                int thisArity = (int)toCreate.GetProperty("Arity").GetValue(null);
                int i = 0; // "Space" left in the tree
                int[] randArray = new int[thisArity];
                object[] arguments = new object[thisArity];
                for(int k = 0; k < thisArity; k++){
                    //generate an array of random numbers used create subtrees of different heights.
                    randArray[k] = random.Next(level);
                }
                for(int j = 0; j < thisArity - 1; j++){
                    arguments[j] = createExpression(expressionTypes0, expressionTypes1, randArray[j] - i);
                    i = j;
                }
                arguments[arguments.Length - 1] = createExpression(expressionTypes0, expressionTypes1, level - i - 1);
                return (Expression)Activator.CreateInstance(toCreate, arguments);
            }
            
        }

        static void Main(string[] args)
        {
            if(args.Length != 3){
                Console.WriteLine("Usage: dotnet run Program [width] [height]");
                return;
            }
            //Get assembly you're running from
            var runningAssembly = Assembly.GetExecutingAssembly();
            var expressionTypes0 = new List<Type>();
            var expressionTypes1 = new List<Type>();

            //If the class can be cast as an Expression, but isn't an interface, add to list
            foreach (var type in runningAssembly.GetTypes()){
                if(!type.IsInterface && !type.IsAbstract && typeof(Expression).IsAssignableFrom(type)){
                    //Check the arity
                    if( (int)type.GetProperty("Arity").GetValue(null) > 0){
                        expressionTypes1.Add(type);
                    }
                    else{
                        expressionTypes0.Add(type);
                    }
                }
            }
            // Start creation of expression tree
            Expression appliedExpression = createExpression(expressionTypes0, expressionTypes1, 20);

            // Take a predefined image for size and format
            Bitmap bmp = new Bitmap(int.Parse(args[1]), int.Parse(args[2]), PixelFormat.Format32bppRgb);
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, bmp.PixelFormat);

            //Scan for the first line
            IntPtr ptr = bmpData.Scan0;

            //Declare an array in which your RGB values will be stored
            int bytes = Math.Abs(bmpData.Stride) * bmp.Height;
            byte[] rgbValues = new byte[bytes];
            float thisHeight = (float)bmp.Height;
            //Copy RGB values in that array
            Marshal.Copy(ptr, rgbValues, 0, bytes);
            Console.Write(appliedExpression.toString() + "\n");
            for(int y = 0; y < bmp.Height; y++){
                for(float x = 0; x < bmp.Width; x++){
                    float u = -1 + (2 * (1 + ( (x - bmp.Width) / bmp.Width) ) );
                    float v = -1 + (2 * (1 + ( (y - thisHeight) / thisHeight) ) );
                    float[] rgb = appliedExpression.eval(u, v);
                    var temp0 = rgb[0] * 127.5 + 127.5;
                    var temp1 = rgb[1] * 127.5 + 127.5;
                    var temp2 = rgb[2] * 127.5 + 127.5;
                    //Console.Write(u + ", " + v + ": [" + rgb[0] + ", " + rgb[1] + ", " + rgb[2] + "], [" + temp0 + ", " + temp1 + ", " + temp2 + "]\n");
                    rgbValues[(int)( (y * bmp.Width + x) * 4)] = (byte)(rgb[0] * 127.5 + 127.5);
                    rgbValues[(int)( (y * bmp.Width + x) * 4) + 1] = (byte)(rgb[1] * 127.5 + 127.5);
                    rgbValues[(int)( (y * bmp.Width + x) * 4) + 2] = (byte)(rgb[2] * 127.5 + 127.5);
                }
            }

            //Copy changed RGB values back to bitmap
            Marshal.Copy(rgbValues, 0, ptr, bytes);

            //Unlock the bits
            bmp.UnlockBits(bmpData);

            bmp.Save("output.png", ImageFormat.Png);
        }
    }
}
