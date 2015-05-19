<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SaldoEnCuentas
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SaldoEnCuentas))
        Me.ToolStripStatusLabel1 = New System.Windows.Forms.ToolStripStatusLabel
        Me.ToolStripProgressBar1 = New System.Windows.Forms.ToolStripProgressBar
        Me.msSaveFileDialog = New System.Windows.Forms.SaveFileDialog
        Me.gbInventario = New System.Windows.Forms.GroupBox
        Me.cmbCuentaDestino = New System.Windows.Forms.ComboBox
        Me.lblCuentaDestino = New System.Windows.Forms.Label
        Me.tcSaldo = New System.Windows.Forms.TabControl
        Me.tpSaldoEnCuentas = New System.Windows.Forms.TabPage
        Me.dgvSaldoEnCuentas = New System.Windows.Forms.DataGridView
        Me.tpEntradas = New System.Windows.Forms.TabPage
        Me.dgvEntradas = New System.Windows.Forms.DataGridView
        Me.tpSalidas = New System.Windows.Forms.TabPage
        Me.dgvSalidas = New System.Windows.Forms.DataGridView
        Me.btnExportarExcel = New System.Windows.Forms.Button
        Me.NotifyIcon1 = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.gbInventario.SuspendLayout()
        Me.tcSaldo.SuspendLayout()
        Me.tpSaldoEnCuentas.SuspendLayout()
        CType(Me.dgvSaldoEnCuentas, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tpEntradas.SuspendLayout()
        CType(Me.dgvEntradas, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tpSalidas.SuspendLayout()
        CType(Me.dgvSalidas, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ToolStripStatusLabel1
        '
        Me.ToolStripStatusLabel1.Name = "ToolStripStatusLabel1"
        Me.ToolStripStatusLabel1.Size = New System.Drawing.Size(244, 17)
        Me.ToolStripStatusLabel1.Text = "% de Información de Proyecto Completada : "
        '
        'ToolStripProgressBar1
        '
        Me.ToolStripProgressBar1.Name = "ToolStripProgressBar1"
        Me.ToolStripProgressBar1.Size = New System.Drawing.Size(100, 16)
        '
        'gbInventario
        '
        Me.gbInventario.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbInventario.Controls.Add(Me.cmbCuentaDestino)
        Me.gbInventario.Controls.Add(Me.lblCuentaDestino)
        Me.gbInventario.Controls.Add(Me.tcSaldo)
        Me.gbInventario.Controls.Add(Me.btnExportarExcel)
        Me.gbInventario.Location = New System.Drawing.Point(4, -2)
        Me.gbInventario.Name = "gbInventario"
        Me.gbInventario.Size = New System.Drawing.Size(689, 529)
        Me.gbInventario.TabIndex = 14
        Me.gbInventario.TabStop = False
        '
        'cmbCuentaDestino
        '
        Me.cmbCuentaDestino.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbCuentaDestino.FormattingEnabled = True
        Me.cmbCuentaDestino.Location = New System.Drawing.Point(87, 19)
        Me.cmbCuentaDestino.Name = "cmbCuentaDestino"
        Me.cmbCuentaDestino.Size = New System.Drawing.Size(252, 21)
        Me.cmbCuentaDestino.TabIndex = 79
        '
        'lblCuentaDestino
        '
        Me.lblCuentaDestino.AutoSize = True
        Me.lblCuentaDestino.Location = New System.Drawing.Point(8, 22)
        Me.lblCuentaDestino.Name = "lblCuentaDestino"
        Me.lblCuentaDestino.Size = New System.Drawing.Size(44, 13)
        Me.lblCuentaDestino.TabIndex = 80
        Me.lblCuentaDestino.Text = "Cuenta:"
        '
        'tcSaldo
        '
        Me.tcSaldo.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tcSaldo.Controls.Add(Me.tpSaldoEnCuentas)
        Me.tcSaldo.Controls.Add(Me.tpEntradas)
        Me.tcSaldo.Controls.Add(Me.tpSalidas)
        Me.tcSaldo.Location = New System.Drawing.Point(12, 55)
        Me.tcSaldo.Name = "tcSaldo"
        Me.tcSaldo.SelectedIndex = 0
        Me.tcSaldo.Size = New System.Drawing.Size(671, 422)
        Me.tcSaldo.TabIndex = 2
        '
        'tpSaldoEnCuentas
        '
        Me.tpSaldoEnCuentas.Controls.Add(Me.dgvSaldoEnCuentas)
        Me.tpSaldoEnCuentas.Location = New System.Drawing.Point(4, 22)
        Me.tpSaldoEnCuentas.Name = "tpSaldoEnCuentas"
        Me.tpSaldoEnCuentas.Padding = New System.Windows.Forms.Padding(3)
        Me.tpSaldoEnCuentas.Size = New System.Drawing.Size(663, 396)
        Me.tpSaldoEnCuentas.TabIndex = 0
        Me.tpSaldoEnCuentas.Text = "Saldo En Cuentas"
        Me.tpSaldoEnCuentas.UseVisualStyleBackColor = True
        '
        'dgvSaldoEnCuentas
        '
        Me.dgvSaldoEnCuentas.AllowUserToAddRows = False
        Me.dgvSaldoEnCuentas.AllowUserToDeleteRows = False
        Me.dgvSaldoEnCuentas.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvSaldoEnCuentas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvSaldoEnCuentas.Location = New System.Drawing.Point(6, 6)
        Me.dgvSaldoEnCuentas.MultiSelect = False
        Me.dgvSaldoEnCuentas.Name = "dgvSaldoEnCuentas"
        Me.dgvSaldoEnCuentas.RowHeadersVisible = False
        Me.dgvSaldoEnCuentas.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me.dgvSaldoEnCuentas.Size = New System.Drawing.Size(651, 387)
        Me.dgvSaldoEnCuentas.TabIndex = 3
        Me.dgvSaldoEnCuentas.Visible = False
        '
        'tpEntradas
        '
        Me.tpEntradas.Controls.Add(Me.dgvEntradas)
        Me.tpEntradas.Location = New System.Drawing.Point(4, 22)
        Me.tpEntradas.Name = "tpEntradas"
        Me.tpEntradas.Size = New System.Drawing.Size(663, 396)
        Me.tpEntradas.TabIndex = 2
        Me.tpEntradas.Text = "Entradas"
        Me.tpEntradas.UseVisualStyleBackColor = True
        '
        'dgvEntradas
        '
        Me.dgvEntradas.AllowUserToAddRows = False
        Me.dgvEntradas.AllowUserToDeleteRows = False
        Me.dgvEntradas.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvEntradas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvEntradas.Location = New System.Drawing.Point(6, 6)
        Me.dgvEntradas.MultiSelect = False
        Me.dgvEntradas.Name = "dgvEntradas"
        Me.dgvEntradas.RowHeadersVisible = False
        Me.dgvEntradas.Size = New System.Drawing.Size(618, 332)
        Me.dgvEntradas.TabIndex = 7
        Me.dgvEntradas.Visible = False
        '
        'tpSalidas
        '
        Me.tpSalidas.Controls.Add(Me.dgvSalidas)
        Me.tpSalidas.Location = New System.Drawing.Point(4, 22)
        Me.tpSalidas.Name = "tpSalidas"
        Me.tpSalidas.Padding = New System.Windows.Forms.Padding(3)
        Me.tpSalidas.Size = New System.Drawing.Size(663, 396)
        Me.tpSalidas.TabIndex = 1
        Me.tpSalidas.Text = "Salidas"
        Me.tpSalidas.UseVisualStyleBackColor = True
        '
        'dgvSalidas
        '
        Me.dgvSalidas.AllowUserToAddRows = False
        Me.dgvSalidas.AllowUserToDeleteRows = False
        Me.dgvSalidas.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvSalidas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvSalidas.Location = New System.Drawing.Point(6, 6)
        Me.dgvSalidas.MultiSelect = False
        Me.dgvSalidas.Name = "dgvSalidas"
        Me.dgvSalidas.RowHeadersVisible = False
        Me.dgvSalidas.Size = New System.Drawing.Size(618, 332)
        Me.dgvSalidas.TabIndex = 8
        Me.dgvSalidas.Visible = False
        '
        'btnExportarExcel
        '
        Me.btnExportarExcel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnExportarExcel.Enabled = False
        Me.btnExportarExcel.Image = Global.Oversight.My.Resources.Resources.excel24x24
        Me.btnExportarExcel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnExportarExcel.Location = New System.Drawing.Point(551, 483)
        Me.btnExportarExcel.Name = "btnExportarExcel"
        Me.btnExportarExcel.Size = New System.Drawing.Size(131, 38)
        Me.btnExportarExcel.TabIndex = 6
        Me.btnExportarExcel.Text = "Exportar a Excel"
        Me.btnExportarExcel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnExportarExcel.UseVisualStyleBackColor = True
        '
        'NotifyIcon1
        '
        Me.NotifyIcon1.Icon = CType(resources.GetObject("NotifyIcon1.Icon"), System.Drawing.Icon)
        Me.NotifyIcon1.Text = "Oversight"
        '
        'SaldoEnCuentas
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(698, 531)
        Me.Controls.Add(Me.gbInventario)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "SaldoEnCuentas"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Saldo En Cuentas"
        Me.gbInventario.ResumeLayout(False)
        Me.gbInventario.PerformLayout()
        Me.tcSaldo.ResumeLayout(False)
        Me.tpSaldoEnCuentas.ResumeLayout(False)
        CType(Me.dgvSaldoEnCuentas, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tpEntradas.ResumeLayout(False)
        CType(Me.dgvEntradas, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tpSalidas.ResumeLayout(False)
        CType(Me.dgvSalidas, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ToolStripStatusLabel1 As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents ToolStripProgressBar1 As System.Windows.Forms.ToolStripProgressBar
    Friend WithEvents msSaveFileDialog As System.Windows.Forms.SaveFileDialog
    Friend WithEvents gbInventario As System.Windows.Forms.GroupBox
    Friend WithEvents tcSaldo As System.Windows.Forms.TabControl
    Friend WithEvents tpSaldoEnCuentas As System.Windows.Forms.TabPage
    Friend WithEvents dgvSaldoEnCuentas As System.Windows.Forms.DataGridView
    Friend WithEvents tpEntradas As System.Windows.Forms.TabPage
    Friend WithEvents dgvEntradas As System.Windows.Forms.DataGridView
    Friend WithEvents tpSalidas As System.Windows.Forms.TabPage
    Friend WithEvents dgvSalidas As System.Windows.Forms.DataGridView
    Friend WithEvents btnExportarExcel As System.Windows.Forms.Button
    Friend WithEvents NotifyIcon1 As System.Windows.Forms.NotifyIcon
    Friend WithEvents cmbCuentaDestino As System.Windows.Forms.ComboBox
    Friend WithEvents lblCuentaDestino As System.Windows.Forms.Label
End Class
