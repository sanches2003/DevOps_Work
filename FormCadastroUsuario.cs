using Repositorio.Entity;
using Repositorio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SalesProject.Consulta;

namespace SalesProject.Cadastro
{
    public partial class FormCadastroUsuario : Form
    {
        private void limparCampos()
        {
            this.txtCodigo.Clear();
            this.txtLogin.Clear();
            this.maskedTextBoxSenha.Clear();
            this.cbAtivo.Checked = false;
        }

        private void habilitarCampos(bool habilita)
        {
            this.txtLogin.Enabled = habilita;
            this.maskedTextBoxSenha.Enabled = habilita;
            this.comboBoxFuncionario.Enabled = habilita;
            this.cbAtivo.Enabled = habilita;
            this.btnSalvar.Enabled = habilita;
            this.btnCadastrarFuncionario.Enabled = habilita;
        }

        private void controlarBotoes(OperadorEnum acao)
        {
            btnCancelar.Enabled = false;
            btnDeletar.Enabled = false;
            btnNovo.Enabled = false;
            btnSalvar.Enabled = false;

            if (acao == OperadorEnum.alterar)
            {
                btnCancelar.Enabled = true;
                btnDeletar.Enabled = true;
                btnSalvar.Enabled = true;
            }
            else
            {
                if (acao == OperadorEnum.inserir)
                {
                    btnCancelar.Enabled = true;
                    btnSalvar.Enabled = true;
                }
                else
                {
                    if (acao == OperadorEnum.inicial)
                    {
                        btnNovo.Enabled = true;
                    }
                }
            }
        }

        private void eventoBotaoNovoClick(object sender, EventArgs e)
        {
            txtCodigo.Enabled = false;

            txtCodigo.Text = "0";

            this.controlarBotoes(OperadorEnum.inserir);

            this.habilitarCampos(true);

            txtLogin.Focus();
        }

        private void eventoBotaoDeletarClick(object sender, EventArgs e)
        {
            int id = int.Parse(txtCodigo.Text);

            var repositorio = new UsuarioRepositorio();

            var obj = repositorio.recuperarPorId(id);

            if (obj != null)
            {
                var sucesso = repositorio.Remover(id);

                if (sucesso)
                {
                    MessageBox.Show("Excluído com sucesso!");
                }
                else
                {
                    MessageBox.Show("Erro ao excluir. Tente novamente.");
                }
            }
            else
            {
                MessageBox.Show("O usuário não existe.");
            }

            this.controlarBotoes(OperadorEnum.inicial);

            this.habilitarCampos(false);

            this.limparCampos();
        }

        private void eventoBotaoCancelarClick(object sender, EventArgs e)
        {
            this.controlarBotoes(OperadorEnum.inicial);

            this.habilitarCampos(false);

            this.limparCampos();
        }

        private bool validarCamposCliente()
        {
            bool sucesso = true;

            if (string.IsNullOrEmpty(txtLogin.Text))
            {
                sucesso = false;

                MessageBox.Show("Informe o Login!");
                return false;
            }

            if (string.IsNullOrEmpty(maskedTextBoxSenha.Text))
            {
                sucesso = false;

                MessageBox.Show("Informe a Senha!");
                return false;
            }

            return sucesso;
        }         

        private void eventoBotaoSalvarClick(object sender, EventArgs e)
        {
            if (this.validarCamposCliente())
            {
                int id = int.Parse(txtCodigo.Text);
                var repositorio = new UsuarioRepositorio();

                Usuario obj = new Usuario();

                obj.Id = id;
                obj.Login = txtLogin.Text;
                obj.Senha = maskedTextBoxSenha.Text;
                obj.IdFuncionario = ((Funcionario)comboBoxFuncionario.Items[comboBoxFuncionario.SelectedIndex]).Id;
                obj.Ativo = cbAtivo.Checked;

                var sucesso = repositorio.Salvar(obj);

                if (sucesso)
                {
                    MessageBox.Show("Salvo com sucesso!");

                }
                else
                {
                    MessageBox.Show("Erro ao salvar. Tente novamente.");
                }

                this.controlarBotoes(OperadorEnum.inicial);

                this.habilitarCampos(false);

                this.limparCampos();
            }
        }

        private void eventoBotaoLocalizarClick(object sender, EventArgs e)
        {
            using (var frm = new FormConsultaUsuario())
            {
                frm.ShowDialog();

                var id = frm.obterIdSelecionado();

                txtCodigo.Text = id.ToString();

                this.controlarBotoes(OperadorEnum.alterar);

                this.habilitarCampos(true);

                this.carregarDados(id);

                txtLogin.Focus();
            }
        }

        public void carregarDados(int id)
        {
            var repositorio = new UsuarioRepositorio();

            var obj = repositorio.recuperarPorId(id);

            if (obj != null)
            {
                txtLogin.Text = obj.Login;
                maskedTextBoxSenha.Text = obj.Senha;
                comboBoxFuncionario.Text = obj.IdFuncionario.ToString();
                cbAtivo.Text = obj.Ativo.ToString();
            }
        }

        public FormCadastroUsuario()
        {
            InitializeComponent();

            this.txtCodigo.Enabled = false;

            this.habilitarCampos(false);

            this.controlarBotoes(OperadorEnum.inicial);

            this.btnNovo.Click += this.eventoBotaoNovoClick;

            this.btnDeletar.Click += this.eventoBotaoDeletarClick;

            this.btnCancelar.Click += this.eventoBotaoCancelarClick;

            this.btnSalvar.Click += this.eventoBotaoSalvarClick;

            this.btnLocalizar.Click += this.eventoBotaoLocalizarClick;

            this.carregarFuncionario();
        }
        private void carregarFuncionario()
        {
            FuncionarioRepositorio objRepositorio = new FuncionarioRepositorio();

            List<Funcionario> listaFuncionario = objRepositorio.listarTodos();

            comboBoxFuncionario.DataSource = listaFuncionario;

            comboBoxFuncionario.DisplayMember = "Nome";
            comboBoxFuncionario.ValueMember = "Id";

        }

        private void btnCadastrarFuncionario_Click_1(object sender, EventArgs e)
        {
            using (var form = new FormCadastroFuncionario())
            {
                form.ShowDialog();
                this.carregarFuncionario();
            }
        }
    }
}

