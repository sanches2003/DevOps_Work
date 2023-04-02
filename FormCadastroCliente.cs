using Repositorio;
using Repositorio.Entity;
using SalesProject.Consulta;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SalesProject
{
    public partial class FormCadastroCliente : Form
    {
        private void limparCampos()
        {
            this.txtCodigo.Clear();
            this.txtNome.Clear();
            this.maskedTextBoxRG.Clear();
            this.maskedTextBoxCPF.Clear();
            this.txtEndereco.Clear();
            this.maskedTextBoxNumero.Clear();
            this.maskedTextBoxTelefone.Clear();
            this.txtEmail.Clear();
            this.txtEstadoCivil.Clear();
            this.txtSexo.Clear();
        }

        private void habilitarCampos(bool habilita)
        {
            this.txtNome.Enabled = habilita;
            this.maskedTextBoxRG.Enabled = habilita;
            this.maskedTextBoxCPF.Enabled = habilita;
            this.txtEndereco.Enabled = habilita;
            this.maskedTextBoxNumero.Enabled = habilita;
            this.maskedTextBoxTelefone.Enabled = habilita;
            this.txtEmail.Enabled = habilita;
            this.dtpDataNascimento.Enabled = habilita;
            this.txtEstadoCivil.Enabled = habilita;
            this.txtSexo.Enabled = habilita;
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

            txtNome.Focus();
        }

        private void eventoBotaoDeletarClick(object sender, EventArgs e)
        {
            int id = int.Parse(txtCodigo.Text);

            var repositorio = new ClienteRepositorio();

            var obj = repositorio.recuperarPorId(id);

            if (obj != null)
            {
                var sucesso = repositorio.remover(id);

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
                MessageBox.Show("O cliente não existe.");
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

        private void eventoBotaoSalvarClick(object sender, EventArgs e)
        {
            if (this.validarCamposCliente())
            {

                int id = int.Parse(txtCodigo.Text);
                var repositorio = new ClienteRepositorio();

                Cliente obj = new Cliente();
                obj.Id = id;
                obj.Nome = txtNome.Text;
                obj.RG = maskedTextBoxRG.Text;
                obj.CPF = maskedTextBoxCPF.Text;
                obj.Endereco = txtEndereco.Text;
                obj.Telefone = maskedTextBoxTelefone.Text;
                obj.Email = txtEmail.Text;
                obj.EstadoCivil = txtEstadoCivil.Text;
                obj.Sexo = txtSexo.Text;
                obj.Numero = int.Parse(maskedTextBoxNumero.Text);
                obj.DataNascimento = DateTime.Parse(dtpDataNascimento.Text);

                var sucesso = repositorio.salvar(obj);

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
            using (var frm = new FormConsultaCliente())
            {
                frm.ShowDialog();

                var id = frm.obterIdSelecionado();

                txtCodigo.Text = id.ToString();

                this.controlarBotoes(OperadorEnum.alterar);

                this.habilitarCampos(true);

                this.carregarDados(id);

                txtNome.Focus();
            }
        }

        public void carregarDados(int id)
        {
            var repositorio = new ClienteRepositorio();

            var obj = repositorio.recuperarPorId(id);

            if (obj != null)
            {
                txtNome.Text = obj.Nome;
                maskedTextBoxRG.Text = obj.RG.ToString();
                maskedTextBoxCPF.Text = obj.CPF.ToString();
                txtEndereco.Text = obj.Endereco;
                maskedTextBoxTelefone.Text = obj.Telefone.ToString();
                txtEmail.Text = obj.Email;
                maskedTextBoxNumero.Text = obj.Numero.ToString();
                txtSexo.Text = obj.Sexo;
                txtEstadoCivil.Text = obj.EstadoCivil;
                dtpDataNascimento.Text = obj.DataNascimento.ToString();
            }
        }

        private bool validarCamposCliente()
        {
            bool sucesso = true;

            if (string.IsNullOrEmpty(txtNome.Text))
            {
                sucesso = false;

                MessageBox.Show("Informe a nome!");
                return false;
            }

            if (string.IsNullOrEmpty(maskedTextBoxCPF.Text))
            {
                sucesso = false;

                MessageBox.Show("Informe o CPF!");
                return false;
            }

            if (string.IsNullOrEmpty(maskedTextBoxRG.Text))
            {
                sucesso = false;

                MessageBox.Show("Informe o RG!");
                return false;
            }

            if (dtpDataNascimento.Value == null)
            {
                sucesso = false;

                MessageBox.Show("Informe a data de nascimento!");
                return false;
            }

            if (string.IsNullOrEmpty(txtEndereco.Text))
            {
                sucesso = false;

                MessageBox.Show("Informe o endereço!");
                return false;
            }

            if (string.IsNullOrEmpty(txtEstadoCivil.Text))
            {
                sucesso = false;

                MessageBox.Show("Informe o Estado Civil!");
                return false;
            }

            if (string.IsNullOrEmpty(txtSexo.Text))
            {
                sucesso = false;

                MessageBox.Show("Informe o Sexo!");
                return false;
            }

             if (string.IsNullOrEmpty(maskedTextBoxNumero.Text))
            {
                sucesso = false;

                MessageBox.Show("Informe o Número!");
                return false;
            }

            if (string.IsNullOrEmpty(maskedTextBoxTelefone.Text))
            {
                sucesso = false;

                MessageBox.Show("Informe o telefone!");
                return false;
            }

            if (string.IsNullOrEmpty(txtEmail.Text))
            {
                sucesso = false;

                MessageBox.Show("Informe o email!");
                return false;
            }

            return sucesso;
        }

        public FormCadastroCliente()
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
        }
    }
}