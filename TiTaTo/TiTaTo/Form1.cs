using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TiTaTo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            buttonsArr = new List<Button> { button1, button2, button3, button4, button5, button6, button7, button8, button9 };
            radioButton1.Checked = true;
        }
        List<Button> buttonsArr; //массив ячеек поля
        //обработчик нажатия на клетку поля
        private void button_click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            move(Convert.ToInt32(button.Tag), human);
        }
        //начальное состояние поля
        string[] board = { "0", "1", "2", "3", "4", "5", "6", "7", "8" };
        string human = "X"; //пользователь по умолчанию
        string comp = "O"; //компьютер по умолчанию
        int iter = 0;
        int roundNum = 0; //номер раунда        
        class steps //объект хода  
        {
            public string index;
            public int points;
        }
        //обработка хода
        public void move(int id, string player)
        {
            if (board[id] != "X" && board[id] != "O")
            {
                roundNum++;
                board[id] = player;
                buttonsArr[id].Enabled = false;
                buttonsArr[id].Text = human;

                if (winning(board, player))
                {
                    DialogResult result = MessageBox.Show("Победили ВЫ!", "Вот так конец!");
                    if (result == DialogResult.OK) newGame();
                    return;
                }
                else if (roundNum > 8)
                {
                    DialogResult result = MessageBox.Show("Да это же НИЧЬЯ!", "Вот так конец!");
                    if (result == DialogResult.OK) newGame();
                    return;
                }
                else
                {
                    roundNum++;
                    var index = Convert.ToInt32(minimax(board, comp).index);
                    buttonsArr[index].Enabled = false;
                    buttonsArr[index].Text = comp;
                    board[index] = comp;

                    if (winning(board, comp))
                    {
                        DialogResult result = MessageBox.Show("Компьютер ПОБЕДИЛ!", "Вот так конец!");
                        if (result == DialogResult.OK) newGame();
                        return;
                    }
                    else if (roundNum == 0)
                    {
                        DialogResult result = MessageBox.Show("Да это же НИЧЬЯ!", "Вот так конец!");
                        if (result == DialogResult.OK) newGame();
                        return;
                    }
                }
            }
        }
        void newGame() //очистка поля для новой игры
        {
            roundNum = 0; int i = 0;
            string[] clearBoard = { "0", "1", "2", "3", "4", "5", "6", "7", "8" };
            foreach (string r in clearBoard)
            {
                board[i] = r;
                i++;
            }
            foreach (var bb in buttonsArr)
            {
                bb.Enabled = true;
                bb.Text = "";
            }
        }
        //алгоритм минимакса
        steps minimax(string[] reboard, string player)
        {
            iter++;
            steps newStep = new steps();
            List<int> emptyСellsArr = (emptyСells(reboard)).ToList();
            if (winning(reboard, human))
            {
                newStep.points = -1;
                return newStep;
            }
            else if (winning(reboard, comp))
            {
                newStep.points = 1;
                return newStep;
            }
            else if (emptyСellsArr.Count == 0)
            {
                newStep.points = 0;
                return newStep;
            }
            List<steps> stepsArr = new List<steps>();
            for (var i = 0; i < emptyСellsArr.Count; i++)
            {
                steps step = new steps();
                step.index = reboard[emptyСellsArr[i]];
                reboard[emptyСellsArr[i]] = player;

                if (player == comp)
                {
                    var minimaxValue = minimax(reboard, human);
                    step.points = minimaxValue.points;
                }
                else
                {
                    var minimaxValue = minimax(reboard, comp);
                    step.points = minimaxValue.points;
                }
                reboard[emptyСellsArr[i]] = step.index;
                stepsArr.Add(step);
            }
            int bestStep = 0;
            if (player == comp)
            {
                var bestPoint = -100;
                for (var i = 0; i < stepsArr.Count; i++)
                {
                    if (stepsArr[i].points > bestPoint)
                    {
                        bestPoint = stepsArr[i].points;
                        bestStep = i;
                    }
                }
            }
            else
            {
                var bestPoint = 100;
                for (var i = 0; i < stepsArr.Count; i++)
                {
                    if (stepsArr[i].points < bestPoint)
                    {
                        bestPoint = stepsArr[i].points;
                        bestStep = i;
                    }
                }
            }
            return stepsArr[bestStep];
        }
        //метод нахождения пустых клеток
        List<int> emptyСells(string[] reboard)
        {
            List<int> emptyСellsArr = new List<int>();
            for (int i = 0; i < reboard.Length; i++)
            {
                if (reboard[i] != "X" && reboard[i] != "O") emptyСellsArr.Add(i);
            }
            return emptyСellsArr;
        }
        //проверка на соответствие выигрышным комбинациям
        bool winning(string[] board, string player)
        {
            if (
              (board[0] == player && board[1] == player && board[2] == player) ||
              (board[3] == player && board[4] == player && board[5] == player) ||
              (board[6] == player && board[7] == player && board[8] == player) ||
              (board[0] == player && board[3] == player && board[6] == player) ||
              (board[1] == player && board[4] == player && board[7] == player) ||
              (board[2] == player && board[5] == player && board[8] == player) ||
              (board[0] == player && board[4] == player && board[8] == player) ||
              (board[2] == player && board[4] == player && board[6] == player)
            ) return true;
            else return false;
        }
        //обработчик выбора Х или О пользователем
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radio = (RadioButton)sender;
            human = radio.Text;
            if (radio.Text.Contains("X")) comp = "O";
            else comp = "X";
        }

    }
}
