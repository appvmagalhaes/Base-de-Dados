using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace BaseDados
{
    public partial class Form1 : Form
    {
        private MySqlConnection mConn; //Conexão
        private MySqlDataAdapter mAdapter; //Adaptador
        private DataSet mDataset; //Dataset
                                  //Definição das variáveis para percorrer os registos
        int inc = 0;
        int Maxrow = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            carregaDados();
        }
        private void carregaDados()
        {
            //Percorrer os registos -------------------
            mDataset = new DataSet();
            mConn = new MySqlConnection("Persist Security " +
                "Info=False; server=localhost; database=clientes; uid=root");
            mConn.Open();
            mAdapter = new MySqlDataAdapter("SELECT cliente.*,postal.localidade" +
                " FROM cliente,postal WHERE cliente.Cod_Post=postal.Cod_Post " +
                "AND cliente.Cod_Zona=postal.Cod_Zona order " +
                "by cliente.id_Cliente", mConn);
            mAdapter.Fill(mDataset, "cliente");
            Navigate();//Chamada do procedimento
            Maxrow = mDataset.Tables["cliente"].Rows.Count;
            mConn.Close();
            codPostal(); //Preenche a ComboBox com Códigos Postais
        }
        private void Navigate()//Procedimento que mostra os dados da navegação
        {
            DataRow drow = mDataset.Tables["cliente"].Rows[inc];
            this.textBox1.Text = drow.ItemArray.GetValue(0).ToString();
            this.textBox2.Text = drow.ItemArray.GetValue(1).ToString();
            this.textBox3.Text = drow.ItemArray.GetValue(2).ToString();
            this.textBox4.Text = drow.ItemArray.GetValue(3).ToString();
            this.textBox5.Text = drow.ItemArray.GetValue(4).ToString();
            this.textBox6.Text = drow.ItemArray.GetValue(5).ToString();
        }
        private void codPostal()
        {
            //Preenche a ComBox com os códigos Postais
            mConn = new MySqlConnection("Persist Security Info=False; server=localhost; database=clientes; uid=root");
            mConn.Open();
            string consulta = "SELECT distinct Cod_Post FROM postal";
            MySqlCommand comando = new MySqlCommand(consulta, mConn);
            MySqlDataReader dr = null;
            dr = comando.ExecuteReader();
            while (dr.Read())
            {
                this.comboBox1.Items.Add(dr.GetString(0));
            }
            dr.Close();
            mConn.Close();
        }

        private void codZonaLocalidade(string codZona)
        {
            //Preenche a ComBox com os códigos de Zona e Localidade
            mConn = new MySqlConnection("Persist Security Info=False; server=localhost; database=clientes; uid=root");
            mConn.Open();
            string consulta = "SELECT Cod_Zona, localidade FROM postal where Cod_Post=" + Convert.ToInt32(codZona);
            MySqlCommand comando = new MySqlCommand(consulta, mConn);
            MySqlDataReader dr = null;
            dr = comando.ExecuteReader();
            this.comboBox2.Items.Clear();
            while (dr.Read())
            {
                this.comboBox2.Items.Add(dr.GetString(0));
            }
            this.textBox6.Text = dr.GetString(1);
            dr.Close();
            mConn.Close();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            mDataset = new DataSet(); //Cria um novo DataSet
            mConn = new MySqlConnection("Persist Security Info=False; " +
                "server=localhost; database=clientes; uid=root");//Liga à BD
                                                                 //  mConn = new MySqlConnection("Persist Security Info=False; " +
                                                                 //   "server=localhost; database=clientes; uid=root; SslMode=none") ;//Liga à BD
            mConn.Open();//Abre a conexão
            if (mConn.State == ConnectionState.Open)//Verifica se há erro na ligação
            {
                //Cria um adaptador com a instrução SQL
                mAdapter = new MySqlDataAdapter("SELECT cliente. id_Cliente AS 'Código Cliente', cliente.nome as Nome, cliente.morada as Morada, cliente.Cod_Post as 'Código Postal', cliente.Cod_Zona as 'Código Zona',postal.localidade as Localidade FROM cliente,postal WHERE cliente.Cod_Post=postal.Cod_Post AND cliente.Cod_Zona=postal.Cod_Zona", mConn);
                //Carrega o resultado na memória
                mAdapter.Fill(mDataset, "dados");
                //Atribui o resultado à DataGridView
                this.dataGridView1.DataSource = mDataset;
                dataGridView1.DataMember = "dados";
            }
            mConn.Close();//Fecha a ligação
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Conexão à BD
            mConn = new MySqlConnection("Persist Security Info=False; " +
                "server=localhost; database=clientes; uid=root");
            // Abre a conexão
            mConn.Open();
            try
            {
                //Query MySQL
                MySqlCommand command = new MySqlCommand("INSERT INTO cliente " +
                    "(id_Cliente,Nome,Morada,Cod_Post,Cod_Zona) VALUES (" + this.textBox1.Text + ",'" +
                    this.textBox2.Text
                   + "','" + this.textBox3.Text + "'," + this.comboBox1.Text + "," + this.comboBox2.Text + ")", mConn);
                //Executa a Query SQL
                command.ExecuteNonQuery();
                //Mensagem de Sucesso
                MessageBox.Show("Gravado com Sucesso!", "Informação", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                this.textBox4.Visible = true;
                this.textBox5.Visible = true;
                this.comboBox1.Visible = false;
                this.comboBox2.Visible = false;
                this.button2.Visible = false;
                this.button11.Visible = true;
                this.button3.Visible = true;
                this.button4.Visible = true;
                this.button5.Visible = true;
                this.button6.Visible = true;
                this.button12.Visible = true;
                this.button7.Visible = true;
                this.button8.Visible = true;
                this.button9.Visible = true;
                this.button10.Visible = true;
                this.button12.Visible = false;
                carregaDados();
            }
            catch
                {
                MessageBox.Show("Erro!!!");
            }
            finally
            {
                // Fecha a conexão
                mConn.Close();
               

            }
        }



        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //Retira o id do cliente quando clica na célula
            string id = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            //Liga à BD
            mConn = new MySqlConnection("Persist Security Info=False; " +
                "server=localhost; " +
                "database=clientes; uid=root");
            //Abre a BD
            mConn.Open();
            //Definição da consulta
            string consulta = "SELECT * FROM cliente where id_Cliente=" +
                Convert.ToInt32(id);
            //Executa a consulta
            MySqlCommand comando = new MySqlCommand(consulta, mConn);
            MySqlDataReader dr = null;
            //Executa a consulta
            dr = comando.ExecuteReader();
            dr.Read();
            //Atribui o resultado
            this.textBox1.Text = dr.GetString(0);
            this.textBox2.Text = dr.GetString(1);
            this.textBox3.Text = dr.GetString(2);
            this.textBox4.Text = dr.GetString(3);
            this.textBox5.Text = dr.GetString(4);
            //Fecha
            dr.Close();
            mConn.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //Conexão à BD
            mConn = new MySqlConnection("Persist Security Info=False; " +
                "server=localhost; database=clientes; uid=root");
            // Abre a conexão
            mConn.Open();
            int confirma = Convert.ToInt32(
                Microsoft.VisualBasic.Interaction.MsgBox("Deseja Realmente Eliminar" +
                " o Corrente Registo?", Microsoft.VisualBasic.MsgBoxStyle.YesNo,
                "Manutenção de Clientes"));
            if (confirma == 6)
            {
                //Query MySQL
                MySqlCommand command = new MySqlCommand("delete from " +
                    "cliente where id_Cliente=" + this.textBox1.Text, mConn);
                //Executa a Query SQL
                command.ExecuteNonQuery();
                //Mensagem de Sucesso
                MessageBox.Show("Dados Eliminados!", "Informação",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
               
            }
            // Fecha a conexão
            mConn.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //Conexão à BD
            mConn = new MySqlConnection("Persist Security Info=False; " +
                "server=localhost; database=clientes; uid=root");
            // Abre a conexão
            mConn.Open();
            int confirma = Convert.ToInt32(Microsoft.VisualBasic.
                Interaction.MsgBox("Deseja Realmente Alterar o Registo?",
                Microsoft.VisualBasic.MsgBoxStyle.YesNo, "Manutenção de Clientes"));
            if (confirma == 6)
            {
                //Query MySQL
                MySqlCommand command = new MySqlCommand("update cliente set nome = '" + this.textBox2.Text + "', morada = '" + this.textBox3.Text + "', " + "cod_Post = " + this.textBox4.Text + ", " + "cod_Zona = " + this.textBox5.Text + " where id_Cliente=" + this.textBox1.Text, mConn);
                //Executa a Query SQL
                command.ExecuteNonQuery();
                //Mensagem de Sucesso
                MessageBox.Show("Registo Alterado!", "Informação",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                carregaDados();
            }
            // Fecha a conexão
            mConn.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (inc != Maxrow - 1)
            {
                inc++;
                Navigate();
            }
            else
            {
                MessageBox.Show("Último");
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (inc != 0)
            {
                inc = 0;
                Navigate();
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (inc > 0)
            {
                inc--;
                Navigate();
            }
            else
            {
                MessageBox.Show("Primeiro");
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (inc != Maxrow - 1)
            {
                inc = Maxrow - 1;
                Navigate();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Form2 frm = new Form2();
            frm.ShowDialog();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            this.textBox1.Clear();
            this.textBox2.Clear();
            this.textBox3.Clear();
            this.textBox4.Clear();
            this.textBox5.Clear();
            this.textBox6.Clear();
            this.textBox1.Focus();

            this.textBox4.Visible = false;
            this.textBox5.Visible = false;
            this.comboBox1.Visible = true;
            this.comboBox2.Visible = true;
            this.button2.Visible = true;
            this.button11.Visible = false;
            this.button3.Visible = false;
            this.button4.Visible = false;
            this.button5.Visible = false;
            this.button6.Visible = false;
            this.button7.Visible = false;
            this.button8.Visible = false;
            this.button9.Visible = false;
            this.button10.Visible = false;
            this.button12.Visible = true;
        }
           
            private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
            {
                //Chama o procedimento para preencher o código de zona e localidade
                codZonaLocalidade(this.comboBox1.SelectedItem.ToString());
            }

        private void button12_Click(object sender, EventArgs e)
        {
            this.textBox4.Visible = true;
            this.textBox5.Visible = true;
            this.comboBox1.Visible = false;
            this.comboBox2.Visible = false;
            this.button2.Visible = false;
            this.button11.Visible = true;
            this.button3.Visible = true;
            this.button4.Visible = true;
            this.button5.Visible = true;
            this.button6.Visible = true;
            this.button12.Visible = false;
            this.button7.Visible = true;
            this.button8.Visible = true;
            this.button9.Visible = true;
            this.button10.Visible = true;
            carregaDados();
        }
    }
    } 
