// C# Using Tags for importing libraries.
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Timers;

namespace UnboundEditor
{
    public partial class unboundLevelEditor : Form
    {
        // Assigning the filepath to locate the JavaScript game files.
        public string filePath = @"c:\Users\iammo\Documents\Projects\Programming 11\ClosedEnvironment\default.js";

        // Creating all code template blocks and containers to interpolate into JavaScript code.
        public string gravity = "0.9";
        public int platformAmount = 1;
        public string objectTemplate = @"platforms.push({x: 100,y: 500,width: 200,height: 50});";

        // Key - Re
        public string keyTemplate = @"if(keyObject.picked == false){ctx.drawImage(key, keyObject.x, keyObject.y, keyObject.width, keyObject.height);}if(keyObject.picked == true) {ctx.drawImage(gate[animFrame], gateObject.x, gateObject.y, gateObject.width, gateObject.height);}";
        public string realObject;
        public string realKey;
        public string realKeyX = "50";
        public string realKeyY = "175";
        public string keyxTemplate;
        public string keyyTemplate;

        public string realGateX = "1250";
        public string realGateY = "200";
        public string gatexTemplate;
        public string gateyTemplate;

        public string playerX = "0";
        public string playerY = "700";
        public string playerH = "50";
        public string playerW = "50";

        // Conditional checks for minor validation.
        public bool platformDraw = false;
        public bool isHovering = false;
        public bool alreadyDonezo = false;
        public bool alreadyGateDonezo = false;

        // Starting a separate thread for looping functions.
        Thread printer2 { get; set; }

        // Variables for platform and mouse XY coordinates. 
        public int[] platHWXY = { 10, 10, 100, 100 };
        public float currentX;
        public float currentY;
        public float currentW;
        public float currentH;

        // Mouse XY coordinates offset.
        public float offsetX = -10;
        public float offsetY = 25;

        // Conditional checks to switch between tools
        public bool platformin = false;
        public bool keyin = false;
        public bool gatin = false;

        // Initial Form Design
        public unboundLevelEditor()
        {
            InitializeComponent();
        }

        // Starting external threads and initializing the picturebox.
        private void Form1_Load(object sender, EventArgs e)
        {
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            pictureBox1.BackgroundImage = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            printer2 = new Thread(new ThreadStart(InvokeMethod));
            printer2.Start();
        }

        // Calling the mouseCoords void every 10 milliseconds to check for conditional functions.
        private void InvokeMethod()
        {
            while (true)
            {
                mouseCoords();
                Thread.Sleep(10);
            }
        }

        // Compile user instructions into JavaScript code.
        private void create_Click(object sender, EventArgs e)
        {
            // Interpolate the base JavaScript code with custom variables.
            string code2 = String.Format(Properties.Settings.Default.defaultCode, gravity, platformAmount, realObject, playerX, playerY, playerH, playerW, realKey, realKeyX, realKeyY, realGateX, realGateY);

            // Modify and compile a new JavaScript file to replace the game's previous code.
            try
            {
                // Read and log the unmodified JavaScript Code (Using StreamReader).
                using (StreamReader sr = File.OpenText(filePath))
                {
                    string s = "";
                    while ((s = sr.ReadLine()) != null)
                    {
                        Console.WriteLine(s);
                    }
                }

                // Create a JavaScript file with the finalized code (using FileStream).
                using (FileStream fs = File.Create(filePath))
                {
                    byte[] info = new UTF8Encoding(true).GetBytes(code2);
                    fs.Write(info, 0, info.Length);
                }

                // Read and log the modified JavaScript code (using StreamReader).
                using (StreamReader sr = File.OpenText(filePath))
                {
                    string s = "";
                    while ((s = sr.ReadLine()) != null)
                    {
                        Console.WriteLine(s);
                    }
                }
            }
            // Catch and log errors.
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        // Change the world gravity from user input.
        private void gravityValue_TextChanged(object sender, EventArgs e)
        {
            gravity = gravityValue.Text;
        }

        // Conditional check for picturebox validation.
        private void pictureBox1_MouseHover(object sender, EventArgs e)
        {
            pictureBox1.Cursor = Cursors.Cross;
            isHovering = true;
        }

        // Looping void that waits for the platform tool instructions.
        public void mouseCoords()
        {
            if (platformin == true)
            {
                if (platformDraw == true)
                {
                    // Draw the editor picturebox background.
                    if (pictureBox1.InvokeRequired)
                    {
                        // Invoking picturebox from external threads.
                        pictureBox1.Invoke(new MethodInvoker(
                        delegate ()
                        {
                            using (var g = Graphics.FromImage(pictureBox1.BackgroundImage))
                            {
                                g.Clear(Color.Black);
                            }
                        }));
                    }
                    else
                    {
                        using (var g = Graphics.FromImage(pictureBox1.BackgroundImage))
                        {
                            g.Clear(Color.Black);
                        }
                    }

                    // Converting a set of mouse XY coordinates to a set of corner XY coordinates and square size.
                    int H = (int)(((currentX + Cursor.Position.X - offsetX) / 2) - (Math.Abs(Cursor.Position.X - currentX - offsetX) / 2));
                    int W = (int)(((currentY + Cursor.Position.Y - offsetY) / 2) - (Math.Abs(Cursor.Position.Y - currentY - offsetY) / 2));
                    int X = (int)(Math.Abs(Cursor.Position.X - currentX - offsetX));
                    int Y = (int)(Math.Abs(Cursor.Position.Y - currentY - offsetY));

                    // Draw a template shape for the platform editor.
                    using (var g = Graphics.FromImage(pictureBox1.BackgroundImage))
                    {
                        g.DrawRectangle(Pens.Red, H, W, X, Y);
                    }

                    // Refresh picture box to draw created shapes.
                    if (pictureBox1.InvokeRequired)
                    {
                        pictureBox1.Invoke(new MethodInvoker(
                        delegate ()
                        {
                            pictureBox1.Refresh();
                        }));
                    }
                    else
                    {
                        pictureBox1.Refresh();
                    }
                }
            }
        }

        // Empty void for future conditional checks.
        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            
        }

        // Ending external threads when the form is closed.

        private void unboundLevelEditor_FormClosed(object sender, FormClosedEventArgs e)
        {
            printer2.Abort();
        }

        // Checking to see if user input is recieved for drawing platforms.
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if(platformin == true)
            {
                currentX = Cursor.Position.X - offsetX;
                currentY = Cursor.Position.Y - offsetY;
                platformDraw = true;
            }
        }
        
        // Creating the final box the top picturebox layer.
        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (platformin == true)
            {
                // Converting a set of mouse XY coordinates to a set of corner XY coordinates and square size.
                platHWXY[0] = (int)(((currentX + Cursor.Position.X - offsetX) / 2) - (Math.Abs(Cursor.Position.X - currentX - offsetX) / 2));
                platHWXY[1] = (int)(((currentY + Cursor.Position.Y - offsetY) / 2) - (Math.Abs(Cursor.Position.Y - currentY - offsetY) / 2));
                platHWXY[2] = (int)(Math.Abs(Cursor.Position.X - currentX - offsetX));
                platHWXY[3] = (int)(Math.Abs(Cursor.Position.Y - currentY - offsetY));

                // Resetting the bool conditional check.
                platformDraw = false;

                // Transferring the Editor platform coordinates and pushing them into the JavaScript code blocks.
                objectTemplate = String.Format(@"platforms.push({{x: {0},y: {1},width: {2},height: {3}}});", platHWXY[0], platHWXY[1], platHWXY[2], platHWXY[3]);
                realObject += " ";
                realObject += objectTemplate;
                platformAmount++;

                // Logging the platform size and coordinates.
                Console.WriteLine(platHWXY[0]);
                Console.WriteLine(platHWXY[1]);
                Console.WriteLine(platHWXY[2]);
                Console.WriteLine(platHWXY[3]);

                // Drawing the box.
                using (var g = Graphics.FromImage(pictureBox1.Image))
                {
                    g.DrawRectangle(Pens.Blue, platHWXY[0], platHWXY[1], platHWXY[2], platHWXY[3]);
                }

                // Refreshing picturebox to display shapes.
                pictureBox1.Refresh();
            }

            // Checking to see if user is trying to create a collectable.
            if(isHovering == true && keyin == true && alreadyDonezo == false)
            {
                // Converting key editor coordinates into JavaScript code blocks.
                keyTemplate = @"if(keyObject.picked == false){{ctx.drawImage(key, keyObject.x, keyObject.y, keyObject.width, keyObject.height);}}if(keyObject.picked == true) {{ctx.drawImage(gate[animFrame], gateObject.x, gateObject.y, gateObject.width, gateObject.height);}}";

                realKey += " ";
                keyxTemplate += Cursor.Position.X - offsetX - 35;
                keyyTemplate += Cursor.Position.Y - offsetY - 30;
                realKeyX = " ";
                realKeyY = " ";

                // Transferring the JavaScript code blocks into the main codebase.
                realKey += keyTemplate;
                realKeyX += keyxTemplate;
                realKeyY += keyyTemplate;

                // Drawing the actual collectable box on the top picturebox layer.
                using (var g = Graphics.FromImage(pictureBox1.Image))
                {
                    g.DrawRectangle(Pens.Green, Cursor.Position.X - offsetX - 35, Cursor.Position.Y - offsetY -30, 50, 50);
                }

                // Refreshing picturebox to show created shapes and resetting conditional checks.
                pictureBox1.Refresh();
                alreadyDonezo = true;
            }

            // Checking if user is trying to create a level exit.
            if(isHovering == true && gatin == true && alreadyGateDonezo == false)
            {
                // Converting editor coordinates into JavaScript code blocks.
                gatexTemplate += Cursor.Position.X - offsetX - 35;
                gateyTemplate += Cursor.Position.Y - offsetY - 30;
                realGateX = " ";
                realGateY = " ";

                // Transferring the JavaScript code blocks into the main codebase.
                realGateX += gatexTemplate;
                realGateY += gateyTemplate;

                // Drawing the actual shape on the top picturebox layer.
                using (var g = Graphics.FromImage(pictureBox1.Image))
                {
                    g.DrawRectangle(Pens.Purple, Cursor.Position.X - offsetX - 35, Cursor.Position.Y - offsetY - 30, 50, 50);
                }

                // Refreshing the picturebox to display the shapes and resetting conditional checks.
                pictureBox1.Refresh();
                alreadyGateDonezo = true;
            }
        }

        // Clearing the level editor.
        private void button1_Click(object sender, EventArgs e)
        {
            // Clearing and resetting all JavaScript codeblocks.
            realObject = " ";
            realKey = " ";
            platformAmount = 1;
            alreadyDonezo = false;
            alreadyGateDonezo = false;
            realKeyX = "50";
            realKeyY = "175";

            // Clearing the top picturebox layer.
            using (var g = Graphics.FromImage(pictureBox1.Image))
            {
                g.Clear(Color.Black);
            }

            // Clearing the bottom picturebox layer.
            using (var g = Graphics.FromImage(pictureBox1.BackgroundImage))
            {
                g.Clear(Color.Black);
            }
            pictureBox1.Refresh();
        }

        // UI textbox text change events to modify world and player variables.

        // Player X Coordinate.
        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            playerX = textBox4.Text;
        }

        // Player Y Coordinate.
        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            playerY = textBox3.Text;
        }

        // Player Height.
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            playerH = textBox2.Text;
        }

        // Player Width.
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            playerW = textBox1.Text;
        }

        // UI radiobutton change events for switching between tools.

        // Platform Tool.
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if(radioButton1.Checked == true)
            {
                platformin = true;
            }
            else
            {
                platformin = false;
            }
        }

        // Collectable Entity Tool.
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked == true)
            {
                keyin = true;
            }
            else
            {
                keyin = false;
            }
        }

        // Level Exit Entity Tool.
        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked == true)
            {
                gatin = true;
            }
            else
            {
                gatin = false;
            }
        }
    }
}
