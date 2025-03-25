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
    public partial class Form2 : Form
    {
        private MySqlConnection mConn; //Conexão
        private MySqlDataAdapter mAdapter; //Adaptador
        private DataSet mDataset; //Dataset

        public Form2()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
                mDataset = new DataSet(); //Cria um novo DataSet
                //Liga à BD
                mConn = new MySqlConnection("Persist Security Info=False; server=localhost; database=clientes; uid=root");
                mConn.Open();//Abre a conexão
                if (mConn.State == ConnectionState.Open)//Verifica se há erro na ligação
                {                                       //Cria um adaptador com a instrução SQL
                    mAdapter = new MySqlDataAdapter("SELECT cliente. id_Cliente" +
                    " AS 'Código Cliente'," +
                    " cliente.nome as Nome," +
                    " cliente.morada as Morada," +
                    " cliente.Cod_Post " +
                    "as 'Código Postal'," +
                    " cliente.Cod_Zona" +
                    " as 'Código Zona'," +
                    "postal.localidade" +
                    " as Localidade FROM " +
                    "cliente,postal WHERE " +
                    "cliente.Cod_Post=postal.Cod_Post AND" +
                    " cliente.Cod_Zona=postal.Cod_Zona " +
                    "and cliente.nome like '" + this.textBox1.Text + "%'", mConn);
                    //Carrega o resultado na memória
                     mAdapter.Fill(mDataset, "dados");
                    //Atribui o resultado à DataGridView
                    this.dataGridView1.DataSource = mDataset;
                    dataGridView1.DataMember = "dados";
                }
            
            mConn.Close();//Fecha a ligação
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            mDataset = new DataSet(); //Cria um novo DataSet
           //Liga à BD
            mConn = new MySqlConnection("Persist Security Info=False; server=localhost; database=clientes; uid=root");
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
    }
}
