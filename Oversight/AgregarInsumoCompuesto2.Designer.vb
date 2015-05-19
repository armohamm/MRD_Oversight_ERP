<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AgregarInsumoCompuesto2
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(AgregarInsumoCompuesto2))
        Me.gbDatosInsumo = New System.Windows.Forms.GroupBox
        Me.tcInsumoCompuesto = New System.Windows.Forms.TabControl
        Me.tpDatosInsumo = New System.Windows.Forms.TabPage
        Me.lblNombreDelInsumo = New System.Windows.Forms.Label
        Me.dgvInsumos = New System.Windows.Forms.DataGridView
        Me.lblCostoParaTabulador = New System.Windows.Forms.Label
        Me.lblTipoDeMaterial = New System.Windows.Forms.Label
        Me.txtNombreDelInsumo = New System.Windows.Forms.TextBox
        Me.btnCancelar = New System.Windows.Forms.Button
        Me.txtCostoParaTabulador = New System.Windows.Forms.TextBox
        Me.btnGuardar = New System.Windows.Forms.Button
        Me.lblUnidadDeMedida = New System.Windows.Forms.Label
        Me.cmbTipoDeInsumo = New System.Windows.Forms.ComboBox
        Me.txtUnidadDeMedida = New System.Windows.Forms.TextBox
        Me.tpTablaPreciosHistoricos = New System.Windows.Forms.TabPage
        Me.dgvPreciosHistoricosInsumo = New System.Windows.Forms.DataGridView
        Me.tpGraficaPreciosHistoricos = New System.Windows.Forms.TabPage
        Me.nccPreciosHistoricosInsumo = New Nevron.Chart.WinForm.NChartControl
        Me.btnEliminarInsumo = New System.Windows.Forms.Button
        Me.btnInsertarInsumo = New System.Windows.Forms.Button
        Me.btnNuevoInsumo = New System.Windows.Forms.Button
        Me.gbDatosInsumo.SuspendLayout()
        Me.tcInsumoCompuesto.SuspendLayout()
        Me.tpDatosInsumo.SuspendLayout()
        CType(Me.dgvInsumos, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tpTablaPreciosHistoricos.SuspendLayout()
        CType(Me.dgvPreciosHistoricosInsumo, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tpGraficaPreciosHistoricos.SuspendLayout()
        Me.SuspendLayout()
        '
        'gbDatosInsumo
        '
        Me.gbDatosInsumo.Controls.Add(Me.tcInsumoCompuesto)
        Me.gbDatosInsumo.Location = New System.Drawing.Point(3, -2)
        Me.gbDatosInsumo.Name = "gbDatosInsumo"
        Me.gbDatosInsumo.Size = New System.Drawing.Size(594, 427)
        Me.gbDatosInsumo.TabIndex = 0
        Me.gbDatosInsumo.TabStop = False
        '
        'tcInsumoCompuesto
        '
        Me.tcInsumoCompuesto.Controls.Add(Me.tpDatosInsumo)
        Me.tcInsumoCompuesto.Controls.Add(Me.tpTablaPreciosHistoricos)
        Me.tcInsumoCompuesto.Controls.Add(Me.tpGraficaPreciosHistoricos)
        Me.tcInsumoCompuesto.Location = New System.Drawing.Point(9, 14)
        Me.tcInsumoCompuesto.Name = "tcInsumoCompuesto"
        Me.tcInsumoCompuesto.SelectedIndex = 0
        Me.tcInsumoCompuesto.Size = New System.Drawing.Size(579, 406)
        Me.tcInsumoCompuesto.TabIndex = 11
        '
        'tpDatosInsumo
        '
        Me.tpDatosInsumo.Controls.Add(Me.btnEliminarInsumo)
        Me.tpDatosInsumo.Controls.Add(Me.btnInsertarInsumo)
        Me.tpDatosInsumo.Controls.Add(Me.btnNuevoInsumo)
        Me.tpDatosInsumo.Controls.Add(Me.lblNombreDelInsumo)
        Me.tpDatosInsumo.Controls.Add(Me.dgvInsumos)
        Me.tpDatosInsumo.Controls.Add(Me.lblCostoParaTabulador)
        Me.tpDatosInsumo.Controls.Add(Me.lblTipoDeMaterial)
        Me.tpDatosInsumo.Controls.Add(Me.txtNombreDelInsumo)
        Me.tpDatosInsumo.Controls.Add(Me.btnCancelar)
        Me.tpDatosInsumo.Controls.Add(Me.txtCostoParaTabulador)
        Me.tpDatosInsumo.Controls.Add(Me.btnGuardar)
        Me.tpDatosInsumo.Controls.Add(Me.lblUnidadDeMedida)
        Me.tpDatosInsumo.Controls.Add(Me.cmbTipoDeInsumo)
        Me.tpDatosInsumo.Controls.Add(Me.txtUnidadDeMedida)
        Me.tpDatosInsumo.Location = New System.Drawing.Point(4, 22)
        Me.tpDatosInsumo.Name = "tpDatosInsumo"
        Me.tpDatosInsumo.Padding = New System.Windows.Forms.Padding(3)
        Me.tpDatosInsumo.Size = New System.Drawing.Size(571, 380)
        Me.tpDatosInsumo.TabIndex = 0
        Me.tpDatosInsumo.Text = "Datos del Insumo"
        Me.tpDatosInsumo.UseVisualStyleBackColor = True
        '
        'lblNombreDelInsumo
        '
        Me.lblNombreDelInsumo.AutoSize = True
        Me.lblNombreDelInsumo.Location = New System.Drawing.Point(6, 12)
        Me.lblNombreDelInsumo.Name = "lblNombreDelInsumo"
        Me.lblNombreDelInsumo.Size = New System.Drawing.Size(101, 13)
        Me.lblNombreDelInsumo.TabIndex = 1
        Me.lblNombreDelInsumo.Text = "Nombre del Insumo:"
        '
        'dgvInsumos
        '
        Me.dgvInsumos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvInsumos.Enabled = False
        Me.dgvInsumos.Location = New System.Drawing.Point(9, 123)
        Me.dgvInsumos.Name = "dgvInsumos"
        Me.dgvInsumos.RowHeadersVisible = False
        Me.dgvInsumos.Size = New System.Drawing.Size(522, 159)
        Me.dgvInsumos.TabIndex = 4
        '
        'lblCostoParaTabulador
        '
        Me.lblCostoParaTabulador.AutoSize = True
        Me.lblCostoParaTabulador.Location = New System.Drawing.Point(347, 298)
        Me.lblCostoParaTabulador.Name = "lblCostoParaTabulador"
        Me.lblCostoParaTabulador.Size = New System.Drawing.Size(112, 13)
        Me.lblCostoParaTabulador.TabIndex = 8
        Me.lblCostoParaTabulador.Text = "Costo para Tabulador:"
        '
        'lblTipoDeMaterial
        '
        Me.lblTipoDeMaterial.AutoSize = True
        Me.lblTipoDeMaterial.Location = New System.Drawing.Point(6, 73)
        Me.lblTipoDeMaterial.Name = "lblTipoDeMaterial"
        Me.lblTipoDeMaterial.Size = New System.Drawing.Size(86, 13)
        Me.lblTipoDeMaterial.TabIndex = 10
        Me.lblTipoDeMaterial.Text = "Tipo de Material:"
        '
        'txtNombreDelInsumo
        '
        Me.txtNombreDelInsumo.Location = New System.Drawing.Point(124, 9)
        Me.txtNombreDelInsumo.Multiline = True
        Me.txtNombreDelInsumo.Name = "txtNombreDelInsumo"
        Me.txtNombreDelInsumo.Size = New System.Drawing.Size(441, 55)
        Me.txtNombreDelInsumo.TabIndex = 1
        '
        'btnCancelar
        '
        Me.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancelar.Image = Global.Oversight.My.Resources.Resources.cancel24x24
        Me.btnCancelar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCancelar.Location = New System.Drawing.Point(184, 332)
        Me.btnCancelar.Name = "btnCancelar"
        Me.btnCancelar.Size = New System.Drawing.Size(197, 34)
        Me.btnCancelar.TabIndex = 6
        Me.btnCancelar.Text = "Cancelar / Regresar sin Cambios"
        Me.btnCancelar.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnCancelar.UseVisualStyleBackColor = True
        '
        'txtCostoParaTabulador
        '
        Me.txtCostoParaTabulador.Enabled = False
        Me.txtCostoParaTabulador.Location = New System.Drawing.Point(465, 295)
        Me.txtCostoParaTabulador.Name = "txtCostoParaTabulador"
        Me.txtCostoParaTabulador.Size = New System.Drawing.Size(100, 20)
        Me.txtCostoParaTabulador.TabIndex = 5
        Me.txtCostoParaTabulador.Text = "0.00"
        '
        'btnGuardar
        '
        Me.btnGuardar.Image = Global.Oversight.My.Resources.Resources.save24x24
        Me.btnGuardar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnGuardar.Location = New System.Drawing.Point(387, 332)
        Me.btnGuardar.Name = "btnGuardar"
        Me.btnGuardar.Size = New System.Drawing.Size(178, 34)
        Me.btnGuardar.TabIndex = 7
        Me.btnGuardar.Text = "Guardar Cambios y Regresar"
        Me.btnGuardar.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnGuardar.UseVisualStyleBackColor = True
        '
        'lblUnidadDeMedida
        '
        Me.lblUnidadDeMedida.AutoSize = True
        Me.lblUnidadDeMedida.Location = New System.Drawing.Point(6, 100)
        Me.lblUnidadDeMedida.Name = "lblUnidadDeMedida"
        Me.lblUnidadDeMedida.Size = New System.Drawing.Size(97, 13)
        Me.lblUnidadDeMedida.TabIndex = 2
        Me.lblUnidadDeMedida.Text = "Unidad de Medida:"
        '
        'cmbTipoDeInsumo
        '
        Me.cmbTipoDeInsumo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbTipoDeInsumo.FormattingEnabled = True
        Me.cmbTipoDeInsumo.Items.AddRange(New Object() {"MATERIALES"})
        Me.cmbTipoDeInsumo.Location = New System.Drawing.Point(124, 70)
        Me.cmbTipoDeInsumo.Name = "cmbTipoDeInsumo"
        Me.cmbTipoDeInsumo.Size = New System.Drawing.Size(138, 21)
        Me.cmbTipoDeInsumo.TabIndex = 2
        '
        'txtUnidadDeMedida
        '
        Me.txtUnidadDeMedida.Location = New System.Drawing.Point(124, 97)
        Me.txtUnidadDeMedida.Name = "txtUnidadDeMedida"
        Me.txtUnidadDeMedida.Size = New System.Drawing.Size(100, 20)
        Me.txtUnidadDeMedida.TabIndex = 3
        '
        'tpTablaPreciosHistoricos
        '
        Me.tpTablaPreciosHistoricos.Controls.Add(Me.dgvPreciosHistoricosInsumo)
        Me.tpTablaPreciosHistoricos.Location = New System.Drawing.Point(4, 22)
        Me.tpTablaPreciosHistoricos.Name = "tpTablaPreciosHistoricos"
        Me.tpTablaPreciosHistoricos.Padding = New System.Windows.Forms.Padding(3)
        Me.tpTablaPreciosHistoricos.Size = New System.Drawing.Size(571, 380)
        Me.tpTablaPreciosHistoricos.TabIndex = 1
        Me.tpTablaPreciosHistoricos.Text = "Precios Históricos Insumo - Tabla"
        Me.tpTablaPreciosHistoricos.UseVisualStyleBackColor = True
        '
        'dgvPreciosHistoricosInsumo
        '
        Me.dgvPreciosHistoricosInsumo.AllowUserToAddRows = False
        Me.dgvPreciosHistoricosInsumo.AllowUserToDeleteRows = False
        Me.dgvPreciosHistoricosInsumo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvPreciosHistoricosInsumo.Location = New System.Drawing.Point(6, 6)
        Me.dgvPreciosHistoricosInsumo.MultiSelect = False
        Me.dgvPreciosHistoricosInsumo.Name = "dgvPreciosHistoricosInsumo"
        Me.dgvPreciosHistoricosInsumo.ReadOnly = True
        Me.dgvPreciosHistoricosInsumo.RowHeadersVisible = False
        Me.dgvPreciosHistoricosInsumo.Size = New System.Drawing.Size(559, 368)
        Me.dgvPreciosHistoricosInsumo.TabIndex = 8
        '
        'tpGraficaPreciosHistoricos
        '
        Me.tpGraficaPreciosHistoricos.Controls.Add(Me.nccPreciosHistoricosInsumo)
        Me.tpGraficaPreciosHistoricos.Location = New System.Drawing.Point(4, 22)
        Me.tpGraficaPreciosHistoricos.Name = "tpGraficaPreciosHistoricos"
        Me.tpGraficaPreciosHistoricos.Size = New System.Drawing.Size(571, 380)
        Me.tpGraficaPreciosHistoricos.TabIndex = 2
        Me.tpGraficaPreciosHistoricos.Text = "Precios Históricos Insumo - Gráfica"
        Me.tpGraficaPreciosHistoricos.UseVisualStyleBackColor = True
        '
        'nccPreciosHistoricosInsumo
        '
        Me.nccPreciosHistoricosInsumo.AutoRefresh = False
        Me.nccPreciosHistoricosInsumo.BackColor = System.Drawing.SystemColors.Control
        Me.nccPreciosHistoricosInsumo.InputKeys = New System.Windows.Forms.Keys(-1) {}
        Me.nccPreciosHistoricosInsumo.Location = New System.Drawing.Point(3, 3)
        Me.nccPreciosHistoricosInsumo.Name = "nccPreciosHistoricosInsumo"
        Me.nccPreciosHistoricosInsumo.Size = New System.Drawing.Size(565, 374)
        Me.nccPreciosHistoricosInsumo.State = CType(resources.GetObject("nccPreciosHistoricosInsumo.State"), Nevron.Chart.WinForm.NState)
        Me.nccPreciosHistoricosInsumo.TabIndex = 9
        Me.nccPreciosHistoricosInsumo.Text = "NChartControl1"
        '
        'btnEliminarInsumo
        '
        Me.btnEliminarInsumo.Image = Global.Oversight.My.Resources.Resources.delete12x12
        Me.btnEliminarInsumo.Location = New System.Drawing.Point(537, 181)
        Me.btnEliminarInsumo.Name = "btnEliminarInsumo"
        Me.btnEliminarInsumo.Size = New System.Drawing.Size(28, 23)
        Me.btnEliminarInsumo.TabIndex = 68
        Me.btnEliminarInsumo.UseVisualStyleBackColor = True
        '
        'btnInsertarInsumo
        '
        Me.btnInsertarInsumo.Image = Global.Oversight.My.Resources.Resources.insertcard12x12
        Me.btnInsertarInsumo.Location = New System.Drawing.Point(537, 152)
        Me.btnInsertarInsumo.Name = "btnInsertarInsumo"
        Me.btnInsertarInsumo.Size = New System.Drawing.Size(28, 23)
        Me.btnInsertarInsumo.TabIndex = 67
        Me.btnInsertarInsumo.UseVisualStyleBackColor = True
        '
        'btnNuevoInsumo
        '
        Me.btnNuevoInsumo.Image = Global.Oversight.My.Resources.Resources.newcard12x12
        Me.btnNuevoInsumo.Location = New System.Drawing.Point(537, 123)
        Me.btnNuevoInsumo.Name = "btnNuevoInsumo"
        Me.btnNuevoInsumo.Size = New System.Drawing.Size(28, 23)
        Me.btnNuevoInsumo.TabIndex = 66
        Me.btnNuevoInsumo.UseVisualStyleBackColor = True
        '
        'AgregarInsumoCompuesto
        '
        Me.AcceptButton = Me.btnGuardar
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCancelar
        Me.ClientSize = New System.Drawing.Size(603, 429)
        Me.Controls.Add(Me.gbDatosInsumo)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "AgregarInsumoCompuesto"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Insumo Compuesto"
        Me.gbDatosInsumo.ResumeLayout(False)
        Me.tcInsumoCompuesto.ResumeLayout(False)
        Me.tpDatosInsumo.ResumeLayout(False)
        Me.tpDatosInsumo.PerformLayout()
        CType(Me.dgvInsumos, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tpTablaPreciosHistoricos.ResumeLayout(False)
        CType(Me.dgvPreciosHistoricosInsumo, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tpGraficaPreciosHistoricos.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents gbDatosInsumo As System.Windows.Forms.GroupBox
    Friend WithEvents lblNombreDelInsumo As System.Windows.Forms.Label
    Friend WithEvents txtNombreDelInsumo As System.Windows.Forms.TextBox
    Friend WithEvents lblUnidadDeMedida As System.Windows.Forms.Label
    Friend WithEvents txtCostoParaTabulador As System.Windows.Forms.TextBox
    Friend WithEvents lblCostoParaTabulador As System.Windows.Forms.Label
    Friend WithEvents txtUnidadDeMedida As System.Windows.Forms.TextBox
    Friend WithEvents btnCancelar As System.Windows.Forms.Button
    Friend WithEvents btnGuardar As System.Windows.Forms.Button
    Friend WithEvents nccPreciosHistoricosInsumo As Nevron.Chart.WinForm.NChartControl
    Friend WithEvents dgvPreciosHistoricosInsumo As System.Windows.Forms.DataGridView
    Friend WithEvents cmbTipoDeInsumo As System.Windows.Forms.ComboBox
    Friend WithEvents lblTipoDeMaterial As System.Windows.Forms.Label
    Friend WithEvents dgvInsumos As System.Windows.Forms.DataGridView
    Friend WithEvents tcInsumoCompuesto As System.Windows.Forms.TabControl
    Friend WithEvents tpDatosInsumo As System.Windows.Forms.TabPage
    Friend WithEvents tpTablaPreciosHistoricos As System.Windows.Forms.TabPage
    Friend WithEvents tpGraficaPreciosHistoricos As System.Windows.Forms.TabPage
    Friend WithEvents btnEliminarInsumo As System.Windows.Forms.Button
    Friend WithEvents btnInsertarInsumo As System.Windows.Forms.Button
    Friend WithEvents btnNuevoInsumo As System.Windows.Forms.Button
End Class
