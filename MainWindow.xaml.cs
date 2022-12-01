using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

/*******************************************************************************
 *  A calculator attempt in C-Sharp language, it behaves as a Windows type     *
 *               calculator,but only the simple mode.                          *
 *  You can use both by clicking the buttons with mouse or keyboard,           *
 *  by clicking i couldn't find any crashes, but when using keyboard sometimes *
 *  it behaves weird, especially after doing a percentage calculation and      *
 *  if you decide to continue another calculation,it can crash or show the     *
 *  result as zero. I know it's about the boolians,there are too many boolians *
 *  to control the situations and i know i'm missing something but i'm really  *
 *  bored :)                                                                  *
 *  i found out that sometimes the enter key calls the reset function but i    * 
 *  couldn't figure why, maybe a broken keyboard is sending wrong signal,      *
 *  who knows...                                                               *     
 *                                                                             *
 *******************************************************************************
 *                             Tamer Kara.2022                                 *
 *******************************************************************************/                             


namespace SimpleCalculatorCSharp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        /******All Variables******/

/*operation variables*/
double sqrt = 0;
        double pwr = 0;
        double inv = 0;
        double p1, p2 = 0;
        double perc, perc2 = 0;
        string backBtn = "";
        string dottemp = "";
        string pe = string.Empty;
        string plusMinus = string.Empty;
        string numPadVal = string.Empty;
        bool operatorClicked = false;
        bool equalClicked = true;
        bool dotClicked = false;
        bool funcClicked = false;
        bool percClicked = false;
        bool keyBoard = false;
        Button buttonOprd = new Button();
        Button btn = new Button();
        /*memory variables*/
        double memory = 0;
        bool memsClicked = false;
        string mem = "";
        Button buttonMem = new Button();

        /*calculating variables*/
        bool numsClicked = false;
        string operandPrevious = "";
        string operandCurrent = "";
        string temp = "";
        double result = 0;
        double num = 0;
        double cntrl = 0;
        int dotCount = 0;
        string number = "";
        string cntrl2 = "";

        /************************/

        public MainWindow()
        {
            InitializeComponent();
            calcScreen.Content = "0";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            btn = (Button)sender;
            handleNumbers(sender, e);
        }
        private void dot_Click(object sender, RoutedEventArgs e)
        {
            dotClicked = true;
        }
        private void Operators_Clicked(object sender, RoutedEventArgs e)
        {
            handleOperators(sender, e);
        }
        private void Equals_Click(object sender, RoutedEventArgs e)
        {
            /*if the percentage operator sign is already at the screen and if the equal button is clicked
             * the operator sign will be erased */
            if (percClicked)
            {
                lblOp.Content = "";
            }

            /*we indicate if the equal button is clicked or not at the end of this function,so the
             default is false and this is the way to start the calculation */
            if (!equalClicked)
            {
                //to begin the calculation the percentage button must also not clicked
                if (!percClicked)

                    /*the last two numbers which used for calculation with the current operator sign
                     is written to the upper screen with equal sign*/
                    lblresult.Content = result.ToString() + operandCurrent + calcScreen.Content + "=";

                //We indicate that any operator clicks are now terminated, as the process has ended
                operatorClicked = false;

                if (numsClicked)
                    calculating();

                result = 0;
                operandPrevious = "";

            }
            //we indicate that the equal button is clicked at least once
            equalClicked = true;
            //if percentage button is clicked in anyhow we must terminate it
            percClicked = false;
        }
        private void c_Click(object sender, RoutedEventArgs e)
        {
            
            //clears the screen and reset everything
            reset();
        }
        private void ce_Click(object sender, RoutedEventArgs e)
        {
            //clears the entry on the screen
            calcScreen.Content = "0";
        }
        private void back_Click(object sender, RoutedEventArgs e)
        {
            //erases the digits on the screen one by one at every clicking 
            temp = (string)calcScreen.Content;
            for (int i = 0; i < temp.Length - 1; i++)
            {
                backBtn += temp[i];
            }
            calcScreen.Content = backBtn;
            backBtn = "";
            if (calcScreen.Content.Equals(""))
            {
                calcScreen.Content = "0";
            }
        }
        private void percentage_Click(object sender, RoutedEventArgs e)
        {
            percentageCalculating();
        }
        private void power_Click(object sender, RoutedEventArgs e)
        {
            //squares the number on the screen
            if (!calcScreen.Content.Equals("0"))
            {
                lblOp.Content = "x²";
                pwr = Math.Pow(Convert.ToDouble(calcScreen.Content), 2);
                calcScreen.Content = pwr.ToString();
            }
            funcClicked = true;
        }
        private void inverse_Click(object sender, RoutedEventArgs e)
        {
            //divides the number on the screen by 1
            if (!calcScreen.Content.Equals("0"))
            {
                double x = Convert.ToDouble(calcScreen.Content);
                lblOp.Content = "1/x";
                inv = Convert.ToDouble(1 / x);
                calcScreen.Content = inv.ToString();
            }
            funcClicked = true;
        }
        private void sroot_Click(object sender, RoutedEventArgs e)
        {
            //takes the squareroot of the number on the screen
            if (!calcScreen.Content.Equals("0"))
            {
                lblOp.Content = "√";
                sqrt = Math.Sqrt(Convert.ToDouble(calcScreen.Content));
                calcScreen.Content = sqrt.ToString();
            }
            funcClicked = true;
        }
        private void btnMemory_Click(object sender, RoutedEventArgs e)
        {
            /*by default MC and MR buttons are not enabled. If M+, M- or MS clicked 
             the MC and MR are enabled. If MC clicked the memory will be erased and 
            MR and MC are disabled again.*/
            buttonMem = (Button)sender;
            mem = (string)buttonMem.Content;
            void btnsEnabled()
            {
                btnMR.IsEnabled = true;
                btnMC.IsEnabled = true;
            }

            switch (mem)
            {
                case "MS":
                    btnsEnabled();
                    lblOp.Content = "MS";
                    lblresult.Content = "";
                    memory = Convert.ToDouble(calcScreen.Content);
                    break;
                case "MR":
                    lblOp.Content = "MR";
                    calcScreen.Content = memory.ToString();
                    break;
                case "MC":
                    lblOp.Content = "MC";
                    memory = 0;
                    calcScreen.Content = "0";
                    btnMR.IsEnabled = false;
                    btnMC.IsEnabled = false;
                    memsClicked = false;
                    break;
                case "M+":
                    lblOp.Content = "M+";
                    btnsEnabled();
                    memory += Convert.ToDouble(calcScreen.Content);
                    break;
                case "M-":
                    lblOp.Content = "M-";
                    btnsEnabled();
                    memory -= Convert.ToDouble(calcScreen.Content);
                    break;
                default:
                    break;
            }
            memsClicked = true;
        }
        private void plusminus_Click(object sender, RoutedEventArgs e)
        {
            //when clicked the number on the screen become negative
            plusMinus = (string)calcScreen.Content;
            calcScreen.Content = "-" + plusMinus;
        }
        private void btnInfo_GotMouseCapture(object sender, MouseEventArgs e)
        {
            lblbasic.Content = "©2022 Tamer Kara";
        }
        private void btnInfo_LostMouseCapture(object sender, MouseEventArgs e)
        {
            lblbasic.Content = "Basic";
        }
        private void calculating()
        {
            switch (operandPrevious)
            {
                case "+":
                    calcScreen.Content = (result + Convert.ToDouble(calcScreen.Content)).ToString();
                    break;
                case "-":
                    calcScreen.Content = (result - Convert.ToDouble(calcScreen.Content)).ToString();
                    break;
                case "×":
                    calcScreen.Content = (result * Convert.ToDouble(calcScreen.Content)).ToString();
                    break;
                case "÷":
                    calcScreen.Content = (result / Convert.ToDouble(calcScreen.Content)).ToString();
                    break;
                default:
                    break;
            }
        }
        private void percentageCalculating()
        {
            /*Because it's not possible to get the percentage of 0, first it must be checked
 if the screen shows not zero*/
            if (!calcScreen.Content.Equals("0"))
            {
                /*The pe value is stored to check if the conditions meet the requirements 
                 * to get the percentage of a number. The variable to be get as the percentage 
                 * is taken from 'pe' value and transfered to p2 excluding the operator sign and 
                 * the percentage value is stored in p1. The perc variable calculates the value 
                 * of ratio.Let's say we want to get %10(p1) of 5(p2), first they are multiplied 
                 * each other and then divided to 100, this gives us 0.5.If the clicked operator 
                 * is '+' or '-', this number is added or subtracted from the original number(p2).
                 * If the clicked operator is multiplication or division then perc operation must 
                 * be done in a different way. p1 variable must be divided to 100, and the result 
                 * must be multiplied or divided with p2 variable.*/
                pe = (string)lblresult.Content;

                if (pe is not null && !pe.Contains("="))
                {
                    lblOp.Content = "%";
                    p2 = Convert.ToDouble(pe.Replace(operandCurrent, ""));

                    perc = (p1 * p2) / 100;

                    switch (operandCurrent)
                    {
                        case "+":
                            perc2 = perc + p2;
                            lblresult.Content = p2 + "+" + perc + "=";
                            calcScreen.Content = perc2.ToString();
                            break;
                        case "-":
                            perc2 = p2 - perc;
                            lblresult.Content = p2 + "-" + perc + "=";
                            calcScreen.Content = perc2.ToString();
                            break;
                        case "×":
                            perc = p1 / 100;
                            perc2 = p2 * perc;
                            lblresult.Content = p2 + "×" + perc + "=";
                            calcScreen.Content = perc2.ToString();
                            break;
                        case "÷":
                            perc = p1 / 100;
                            perc2 = p2 / perc;
                            lblresult.Content = p2 + "÷" + perc + "=";
                            calcScreen.Content = perc2.ToString();
                            break;

                        default:
                            break;
                    }
                    percClicked = true;
                    operatorClicked = false;
                    numsClicked = false;
                }
            }
            else if (!operatorClicked)
                percClicked = true;
        }
        private void reset()
        {
            
                result = 0;
                cntrl = 0;
                cntrl2 = "";
                pe = "";
                plusMinus = "";
                numPadVal = "";
                temp = "";
                number = "";
                p1 = 0;
                p2 = 0;
                perc = 0;
                perc2 = 0;
                sqrt = 0;
                pwr = 0;
                inv = 0;
                num = 0;
                dotCount = 0;
                calcScreen.Content = "0";
                lblresult.Content = "";
                lblOp.Content = "";
                operandPrevious = "";
                operandCurrent = "";
                operatorClicked = false;
                equalClicked = false;
                dotClicked = false;
                funcClicked = false;
                numsClicked = false;
                percClicked = false;
                memsClicked = false;
                keyBoard = false;
            
        }
        private void commaCount(string dottemp)
        {
            //Counts how many commas on the entered number
            foreach (Char c in dottemp)
            {
                if (c == ',')
                {
                    dotCount++;
                }
            }
        }
        private void handleNumbers(object sender, RoutedEventArgs e)
        {
            /*if percentage clicked while the screen is in use, reset function called
            to prevent an error caused at cntrl2 variable*/
            if (percClicked)
            {
                reset();
            }

            cntrl2 = (string)calcScreen.Content;

            /*if a new calculation needed after clicking of equal and the result is already at 
             * screen and if the new number is going to be a decimal number less than 1 and if 
             * it's going to be written without clicking zero but just comma, the screen must 
             * be cleaned and 0 must be written on screen to avoid using the old number on screen */
            if (equalClicked)
            {
                calcScreen.Content = "0";
            }

            /*If 'memory', 'square', 'squareroot' and 'inverse' is used, to be entered a new number 
             * after these operations main screen must be cleared, if not cleared the last pressed 
             * digit will be added to the existing number, which is not preferred.*/

            if (lblOp.Content is not null)
            {
                if (lblOp.Content.Equals("M+") || lblOp.Content.Equals("M-") || lblOp.Content.Equals("MR") || lblOp.Content.Equals("MC")
                    || lblOp.Content.Equals("MS") || lblOp.Content.Equals("x²") || lblOp.Content.Equals("√")
                    || lblOp.Content.Equals("1/x"))
                {
                    calcScreen.Content = "";
                    lblOp.Content = "";
                }

            }

            /*if the decimal button clicked, first of all the number on the screen stored in a 
             * temporary variable called dottemp, then using commaCount function by using dottemp 
             * variable the dotCount variable returns the value 'dotCount'. dotCount detects if
             the number on the screen contains a comma, if the value is 1 and the operandClicked is 
            not clicked, the button sender's value is stored in a variable called "cntrl",and the 
            "cntrl2" value is checked which stores the screen's value just after a button click at 
            beginning, if the value of cntrl is bigger then 1 and cntrl2's value not contains a comma,
            a comma will be added to the number exists on the screen, else if the controllers don't 
            match then the number on the screen will be erased and the button sender's number will be
            add after a zero and a comma. (Ex. 0.5) */

            if (dotClicked)
            {
                dottemp = (string)calcScreen.Content;
                commaCount(dottemp);

                if (dotCount < 1 || !operatorClicked)
                {
                    cntrl = Convert.ToDouble(btn.Content);
                    if ((cntrl > 1 && !cntrl2.Contains(',')) || dotClicked)
                        calcScreen.Content = calcScreen.Content + "," + (string)btn.Content;
                    else
                        calcScreen.Content = "0" + "," + (string)btn.Content;
                }

                dotClicked = false;
                funcClicked = false;
            }

            /*At the beginning if the screen contains a 0 or a number clicked(numsClicked) or the
             equal button clicked(equalClicked) or a mathematical function button is clicked as pwr,
            sqrt or inverse(funcClicked) or the memory buttons clicked(memsClicked) then the screen
            content and the the process screen upside is cleared and the pressed button will appear
            on screen*/

            else
            {
                if (calcScreen.Content.Equals("0") || !numsClicked || equalClicked || funcClicked)
                {
                    calcScreen.Content = "";
                    lblOp.Content = "";
                }
                if (btn.IsPressed)
                {
                    calcScreen.Content += (string)btn.Content;
                }
                else
                {
                    calcScreen.Content += (string)numPadVal;
                }

            }

            numsClicked = true;
            equalClicked = false;
            operatorClicked = false;

            //this value is stored for the percentage calculation
            try
            {
                p1 = Convert.ToDouble(calcScreen.Content);
            }
            catch (Exception)
            {

                calcScreen.Content = "Err";
            }

            
            cntrl2 = "";

        }
        private void handleOperators(object sender, RoutedEventArgs e)
        {
            /*This method catches which operator sign is clicked(+,-,*,/)
            * and if already a number is clicked,it writes the operand and the
            operator to the upper process screen.With 'calculating' method the
            calculation is made according to the operator sign. And the result
            is written to the main screen.*/

            //confirms the operator click
            operatorClicked = true;
            //decimal numbers are off by default
            dotClicked = false;
            /*percClicked is set to false during operator click, 
             * if it is set to true and you try to make an operation 
             * after percentage calculation, the screen will reset when 
             * clicking a new number and it become impossible to make an 
             * operation using the existing percentage calculation result.*/

            percClicked = false;

            if (keyBoard == false)
            {
                buttonOprd = (Button)sender;
            }

            if (buttonOprd.IsPressed)
            {

                //the current operator sign stored
                operandCurrent = (string)buttonOprd.Content;
            }
            else
            {
                if (numPadVal.Equals("*"))
                    operandCurrent = "×";
                else if (numPadVal.Equals("/"))
                    operandCurrent = "÷";
                else
                    operandCurrent = numPadVal;
            }


            /*if a number button clicked, the number on the lower screen 
             * transferred to the  upper screen with the current operator
             * sign included*/

            if (numsClicked)
            {
                lblresult.Content = calcScreen.Content + "" + operandCurrent;
                //mathematical operation occurs 
                calculating();

            }

            try
            {
                //after calculation the result is transferred to the variable 'result'
                result = Convert.ToDouble(calcScreen.Content);
            }
            catch (Exception)
            {
                //if any error occurs screen shows a message 'error'
                lblresult.Content = "";
                calcScreen.Content = "error";
            }

            //the result is written at upper screen
            lblresult.Content = result + operandCurrent;

            /*to able to make the calculation and write it on the screen
             it needs the previous operand,so it stores that value by passing
            the current value to a previous variable 'operandPrevious' */
            operandPrevious = operandCurrent;
            //existing operator clicking is terminated
            operatorClicked = false;
            //existing number button click state is terminated
            numsClicked = false;
            //if clicked, the memory butons click state is terminated
            memsClicked = false;
        }
        private void numberDict(object sender, KeyEventArgs e)
        {
            Dictionary<Key, string> numbers = new Dictionary<Key, string>();
            numbers.Add(Key.NumPad0, "0");
            numbers.Add(Key.NumPad1, "1");
            numbers.Add(Key.NumPad2, "2");
            numbers.Add(Key.NumPad3, "3");
            numbers.Add(Key.NumPad4, "4");
            numbers.Add(Key.NumPad5, "5");
            numbers.Add(Key.NumPad6, "6");
            numbers.Add(Key.NumPad7, "7");
            numbers.Add(Key.NumPad8, "8");
            numbers.Add(Key.NumPad9, "9");
            numbers.Add(Key.D0, "0");
            numbers.Add(Key.D1, "1");
            numbers.Add(Key.D2, "2");
            numbers.Add(Key.D3, "3");
            numbers.Add(Key.D4, "4");
            numbers.Add(Key.D5, "5");
            numbers.Add(Key.D6, "6");
            numbers.Add(Key.D7, "7");
            numbers.Add(Key.D8, "8");
            numbers.Add(Key.D9, "9");
            numbers.Add(Key.OemMinus, "-");
            numbers.Add(Key.Oem8, "*");
            numbers.Add(Key.Decimal, ",");
            numbers.Add(Key.Multiply, "*");
            numbers.Add(Key.Divide, "/");
            numbers.Add(Key.Add, "+");
            numbers.Add(Key.Subtract, "-");
            numbers.Add(Key.Enter, "=");
            numbers.Add(Key.Back, "←");

            foreach (var item in numbers)
            {
                if (e.Key == item.Key)
                {
                    numPadVal = item.Value;
                }
            }
        }
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            keyBoard = true;

            numberDict(sender, e);

            if (numPadVal == "+" || numPadVal == "-" || numPadVal == "*" || numPadVal == "/")
            {
                handleOperators(sender, e);
            }

            else
            {
                if (numPadVal == "=")
                {
                    Equals_Click(sender, e);
                }

                else if (numPadVal == "←")
                {
                    back_Click(sender, e);
                }

                else
                {
                    if (numPadVal == ",")
                        dotClicked = true;

                    handleNumbers(sender,e);
                    dotClicked = false;
                }

                keyBoard = false;
            }
        }
     }
   }


