<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class BuscaVales
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(BuscaVales))
        Me.gbBuscar = New System.Windows.Forms.GroupBox
        Me.btnExportar = New System.Windows.Forms.Button
        Me.btnCrear = New System.Windows.Forms.Button
        Me.dgvVales = New System.Windows.Forms.DataGridView
        Me.btnAbrir = New System.Windows.Forms.Button
        Me.btnCancelar = New System.Windows.Forms.Button
        Me.lblBuscar = New System.Windows.Forms.Label
        Me.txtBuscar = New System.Windows.Forms.TextBox
        Me.NotifyIcon1 = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.msSaveFileDialog = New System.Windows.Forms.SaveFileDialog
        Me.gbBuscar.SuspendLayout()
        CType(Me.dgvVales, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'gbBuscar
        '
        Me.gbBuscar.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbBuscar.Controls.Add(Me.btnExportar)
        Me.gbBuscar.Controls.Add(Me.btnCrear)
        Me.gbBuscar.Controls.Add(Me.dgvVales)
        Me.gbBuscar.Controls.Add(Me.btnAbrir)
        Me.gbBuscar.Controls.Add(Me.btnCancelar)
        Me.gbBuscar.Controls.Add(Me.lblBuscar)
        Me.gbBuscar.Controls.Add(Me.txtBuscar)
        Me.gbBuscar.Location = New System.Drawing.Point(7, 2)
        Me.gbBuscar.Name = "gbBuscar"
        Me.gbBuscar.Size = New System.Drawing.Size(1023, 413)
        Me.gbBuscar.TabIndex = 29
        Me.gbBuscar.TabStop = False
        '
        'btnExportar
        '
        Me.btnExportar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnExportar.Enabled = False
        Me.btnExportar.Image = Global.Oversight.My.Resources.Resources.excel24x24
        Me.btnExportar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnExportar.Location = New System.Drawing.Point(161, 373)
        Me.btnExportar.Name = "btnExportar"
        Me.btnExportar.Size = New System.Drawing.Size(107, 34)
        Me.btnExportar.TabIndex = 30
        Me.btnExportar.Text = "Exportar lista"
        Me.btnExportar.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnExportar.UseVisualStyleBackColor = True
        '
        'btnCrear
        '
        Me.btnCrear.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnCrear.Enabled = False
        Me.btnCrear.Image = Global.Oversight.My.Resources.Resources.gas24x22
        Me.btnCrear.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCrear.Location = New System.Drawing.Point(11, 373)
        Me.btnCrear.Name = "btnCrear"
        Me.btnCrear.Size = New System.Drawing.Size(144, 34)
        Me.btnCrear.TabIndex = 3
        Me.btnCrear.Text = "C&rear Nuevo Vale"
        Me.btnCrear.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnCrear.UseVisualStyleBackColor = True
        '
        'dgvVales
        '
        Me.dgvVales.AllowUserToAddRows = False
        Me.dgvVales.AllowUserToDeleteRows = False
        Me.dgvVales.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvVales.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText
        Me.dgvVales.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvVales.Location = New System.Drawing.Point(11, 47)
        Me.dgvVales.MultiSelect = False
        Me.dgvVales.Name = "dgvVales"
        Me.dgvVales.ReadOnly = True
        Me.dgvVales.RowHeadersVisible = False
        Me.dgvVales.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvVales.Size = New System.Drawing.Size(1006, 320)
        Me.dgvVales.TabIndex = 2
        '
        'btnAbrir
        '
        Me.btnAbrir.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnAbrir.Enabled = False
        Me.btnAbrir.Image = Global.Oversight.My.Resources.Resources.open24x24
        Me.btnAbrir.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnAbrir.Location = New System.Drawing.Point(905, 373)
        Me.btnAbrir.Name = "btnAbrir"
        Me.btnAbrir.Size = New System.Drawing.Size(112, 34)
        Me.btnAbrir.TabIndex = 5
        Me.btnAbrir.Text = "&Utilizar Vale"
        Me.btnAbrir.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnAbrir.UseVisualStyleBackColor = True
        '
        'btnCancelar
        '
        Me.btnCancelar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancelar.Image = Global.Oversight.My.Resources.Resources.cancel24x24
        Me.btnCancelar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCancelar.Location = New System.Drawing.Point(810, 373)
        Me.btnCancelar.Name = "btnCancelar"
        Me.btnCancelar.Size = New System.Drawing.Size(89, 34)
        Me.btnCancelar.TabIndex = 4
        Me.btnCancelar.Text = "&Cancelar"
        Me.btnCancelar.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnCancelar.UseVisualStyleBackColor = True
        '
        'lblBuscar
        '
        Me.lblBuscar.AutoSize = True
        Me.lblBuscar.Location = New System.Drawing.Point(8, 18)
        Me.lblBuscar.Name = "lblBuscar"
        Me.lblBuscar.Size = New System.Drawing.Size(43, 13)
        Me.lblBuscar.TabIndex = 25
        Me.lblBuscar.Text = "Buscar:"
        '
        'txtBuscar
        '
        Me.txtBuscar.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtBuscar.Location = New System.Drawing.Point(57, 15)
        Me.txtBuscar.Name = "txtBuscar"
        Me.txtBuscar.Size = New System.Drawing.Size(960, 20)
        Me.txtBuscar.TabIndex = 1
        '
        'NotifyIcon1
        '
        Me.NotifyIcon1.Icon = CType(resources.GetObject("NotifyIcon1.Icon"), System.Drawing.Icon)
        Me.NotifyIcon1.Text = "Oversight"
        '
        'BuscaVales
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1037, 418)
        Me.Controls.Add(Me.gbBuscar)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "BuscaVales"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Buscar Vales de Gasolina"
        Me.gbBuscar.ResumeLayout(False)
        Me.gbBuscar.PerformLayout()
        CType(Me.dgvVales, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents gbBuscar As System.Windows.Forms.GroupBox
    Friend WithEvents btnAbrir As System.Windows.Forms.Button
    Friend WithEvents btnCancelar As System.Windows.Forms.Button
    Friend WithEvents dgvVales As System.Windows.Forms.DataGridView
    Friend WithEvents lblBuscar As System.Windows.Forms.Label
    Friend WithEvents txtBuscar As System.Windows.Forms.TextBox
    Friend WithEvents btnCrear As System.Windows.Forms.Button
    Friend WithEvents NotifyIcon1 As System.Windows.Forms.NotifyIcon
    Friend WithEvents btnExportar As System.Windows.Forms.Button
    Friend WithEvents msSaveFileDialog As System.Windows.Forms.SaveFileDialog
End Class
