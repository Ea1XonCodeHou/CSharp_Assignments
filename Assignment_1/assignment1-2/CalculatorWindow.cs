using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Assignment1_2_windowsProject
{
    public partial class CalculatorWindow : Form
    {
        public CalculatorWindow()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void firstNumberBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void operatorButton_Click(object sender, EventArgs e)
        {
            try
            {
                double firstNumber = Convert.ToDouble(firstNumberBox.Text);
                double secondNumber = Convert.ToDouble(secondNumberBox.Text);

                string selectedOperator = operatorCheckBox.SelectedItem?.ToString();

                if (string.IsNullOrEmpty(selectedOperator))
                {
                    MessageBox.Show("请选择一个运算符！");
                    return;
                }

                double result = 0; // 存储计算结果

                // 进行运算
                switch (selectedOperator)
                {
                    case "+":
                        result = firstNumber + secondNumber;
                        break;
                    case "-":
                        result = firstNumber - secondNumber;
                        break;
                    case "*":
                        result = firstNumber * secondNumber;
                        break;
                    case "/":
                        if (secondNumber != 0)
                            result = firstNumber / secondNumber;
                        else
                        {
                            MessageBox.Show("除数不能为 0！");
                            return;
                        }
                        break;
                    case "%":
                        result = firstNumber % secondNumber;
                        break;
                    default:
                        MessageBox.Show("无效的运算符！");
                        return;
                }

                // 显示计算结果
                ResultLine.Text = result.ToString();
            }
            catch (FormatException)
            {
                MessageBox.Show("请输入有效的数字！");
            }
        }

    }
}
