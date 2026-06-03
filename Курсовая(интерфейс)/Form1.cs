using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Курсовая_интерфейс_
{
    public partial class Form1 : Form
    {
        private List<int> heaps; // Список куч
        private bool isPlayerTurn; // Чей ход (true - игрок, false - компьютер)
        private bool gameActive; // Активна ли игра
        private Random random; // Для случайных ходов компьютера

        public Form1()
        {
            InitializeComponent();
            random = new Random();
            InitializeGameState();
        }
        private void InitializeGameState()
        {
            heaps = new List<int>(); // Создает новый пустой список для хранения куч
            isPlayerTurn = true;  // Устанавливает, что ходит игрок (true - игрок, false - компьютер)
            gameActive = false; // Игра пока не активна (не начата)
            // Включаем кнопку хода только когда игра активна и ход игрока
            button3.Enabled = false;
            // Очищаем текстовые поля
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            textBox7.Clear();
            textBox8.Clear();
        }

        private void UpdateGameDisplay()
        {
            // Отображаем текущее состояние куч
            textBox2.Clear();
            if (heaps.Count == 0)
            {
                textBox2.AppendText("Куч нет - игра завершена!\r\n");
                if (gameActive)
                {
                    gameActive = false;
                    button3.Enabled = false;
                    if (!isPlayerTurn) MessageBox.Show("Вы победили! Игрок не смог сделать ход.", "Победа!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        else MessageBox.Show("Компьютер победил! Вы не смогли сделать ход.", "Поражение!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
                            else for (int i = 0; i < heaps.Count; i++)
                                        textBox2.AppendText($"Куча {i + 1}: {heaps[i]} камней\r\n");
                                                                        
            // Обновляем доступность кнопки хода
            if (gameActive && isPlayerTurn) button3.Enabled = true;
                else button3.Enabled = false;
        }

        // Проверка корректности хода
        private bool IsValidMove(int heapIndex, int part1, int part2)
        {
            // Проверка индекса кучи
            if (heapIndex < 1 || heapIndex > heaps.Count) return false;
            int heapSize = heaps[heapIndex - 1];
            // Проверка: куча должна быть больше 1 (т.к. нужно разделить на 2 части)
            if (heapSize <= 1) return false;
            // Проверка: части должны быть положительными
            if (part1 <= 0 || part2 <= 0) return false;
            // Проверка: сумма частей должна равняться размеру кучи
            if (part1 + part2 != heapSize) return false;
            return true;
        }

        private void MakeMove(int heapIndex, int part1, int part2, bool isComputer)
        {
            // Удаляем выбранную кучу
            heaps.RemoveAt(heapIndex - 1);
            // Добавляем две новые кучи (если они не нулевые)
            if (part1 > 0) heaps.Insert(heapIndex - 1, part1);
                else heapIndex--; // Корректировка индекса

            if (part2 > 0)
            {
                if (part1 > 0) heaps.Insert(heapIndex, part2);
                    else heaps.Insert(heapIndex - 1, part2);
            }
            // Очищаем поля ввода хода человека
            if (!isComputer)
            {
                textBox6.Clear();
                textBox7.Clear();
                textBox8.Clear();
            }
            // Меняем игрока
            isPlayerTurn = !isPlayerTurn;
            // Обновляем отображение
            UpdateGameDisplay();
            // Если игра активна и ход компьютера, делаем ход компьютера
            if (gameActive && !isPlayerTurn) MakeComputerMove();
        }

        // Ход компьютера
        private void MakeComputerMove()
        {
            if (!gameActive || isPlayerTurn) return;
            // Проверяем, есть ли возможные ходы
            if (!HasAnyValidMove())
            {
                // Нет возможных ходов - компьютер проиграл
                gameActive = false;
                UpdateGameDisplay();
                return;
            }
            // Находим все возможные ходы
            List<Tuple<int, int, int>> possibleMoves = GetAllPossibleMoves();
            // Выбираем случайный ход
            var move = possibleMoves[random.Next(possibleMoves.Count)];
            int heapIndex = move.Item1;
            int part1 = move.Item2;
            int part2 = move.Item3;

            // Отображаем ход компьютера
            textBox3.Text = heapIndex.ToString();
            textBox4.Text = part1.ToString();
            textBox5.Text = part2.ToString();

            // Выполняем ход
            MakeMove(heapIndex, part1, part2, true);
        }

        // Проверка наличия хотя бы одного возможного хода
        private bool HasAnyValidMove()
        {
            for (int i = 0; i < heaps.Count; i++)
            {
                if (heaps[i] > 1) // Кучу можно разделить только если в ней больше 1 камня
                {
                    // Проверяем, можно ли разделить на две положительные части
                    for (int part1 = 1; part1 <= heaps[i] - 1; part1++)
                    {
                        int part2 = heaps[i] - part1;
                        if (part1 > 0 && part2 > 0) return true;
                    }
                }
            }
            return false;
        }



        //Найти все возможные ходы
        private List<Tuple<int, int, int>> GetAllPossibleMoves()
        {
            List<Tuple<int, int, int>> moves = new List<Tuple<int, int, int>>();
            for (int i = 0; i < heaps.Count; i++)
            {
                int heapSize = heaps[i];
                if (heapSize > 1)
                {
                    for (int part1 = 1; part1 <= heapSize - 1; part1++)
                    {
                        int part2 = heapSize - part1;
                        if (part1 != part2)  moves.Add(Tuple.Create(i + 1, part1, part2)); // Правило: части не должны быть равны
                    }
                }
            }
            return moves;
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы точно хотите всё сбросить?", "Подтверждение сброса",
        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                textBox1.Clear();
                textBox2.Clear();
                textBox3.Clear();
                textBox4.Clear();
                textBox5.Clear();
                textBox6.Clear();
                textBox7.Clear();
                textBox8.Clear();
                InitializeGameState();
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            textBox2.ReadOnly = true;
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            textBox3.ReadOnly = true;
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            textBox4.ReadOnly = true;
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            textBox5.ReadOnly = true;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar)) e.Handled = true;
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            textBox2.ReadOnly = true;
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            textBox3.ReadOnly = true;
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            textBox4.ReadOnly = true;
        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            textBox5.ReadOnly = true;
        }

        private void button1_Click(object sender, EventArgs e) //Кнопка "Начать игру"
        {
            //Проверка кучи
            if (string.IsNullOrWhiteSpace(textBox1.Text))
                                                        {
                                                            MessageBox.Show("Введите размер кучи!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                                            return;
                                                        }
            int heapSize = int.Parse(textBox1.Text);
            if (!int.TryParse(textBox1.Text, out heapSize) || heapSize <= 2)
                                                                            {
                                                                                MessageBox.Show("Размер кучи должен быть целым числом больше 2!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                                                                return;
                                                                            }
            //Проверка первого хода
            if (!radioButton1.Checked && !radioButton2.Checked)
            {
                MessageBox.Show("Выберите, кто ходит первым!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

        }

        private void textBox8_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar)) e.Handled = true;
        }

        private void textBox7_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar)) e.Handled = true;
        }

        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar)) e.Handled = true;
        }
    }
}