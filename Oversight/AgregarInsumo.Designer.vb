<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AgregarInsumo
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(AgregarInsumo))
        Me.gbDatosInsumo = New System.Windows.Forms.GroupBox
        Me.tcInsumo = New System.Windows.Forms.TabControl
        Me.tpDatosInsumo = New System.Windows.Forms.TabPage
        Me.lblCategoriaInsumo = New System.Windows.Forms.Label
        Me.txtCategoria = New System.Windows.Forms.TextBox
        Me.lblPrecioPieCubico = New System.Windows.Forms.Label
        Me.lblLargo = New System.Windows.Forms.Label
        Me.lblAncho = New System.Windows.Forms.Label
        Me.lblEspesor = New System.Windows.Forms.Label
        Me.txtPrecioPieCubico = New System.Windows.Forms.TextBox
        Me.txtLargo = New System.Windows.Forms.TextBox
        Me.txtAncho = New System.Windows.Forms.TextBox
        Me.txtEspesor = New System.Windows.Forms.TextBox
        Me.chkCubicar = New System.Windows.Forms.CheckBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.lblNombreDelInsumo = New System.Windows.Forms.Label
        Me.cmbTipoDeInsumo = New System.Windows.Forms.ComboBox
        Me.txtNombreDelInsumo = New System.Windows.Forms.TextBox
        Me.btnCancelar = New System.Windows.Forms.Button
        Me.lblUnidadDeMedida = New System.Windows.Forms.Label
        Me.txtCostoParaTabulador = New System.Windows.Forms.TextBox
        Me.txtUnidadDeMedida = New System.Windows.Forms.TextBox
        Me.btnGuardar = New System.Windows.Forms.Button
        Me.lblCostoSINIVA = New System.Windows.Forms.Label
        Me.lblCostoParaTabulador = New System.Windows.Forms.Label
        Me.txtCostoSINIVA = New System.Windows.Forms.TextBox
        Me.txtPorcentajeDeProteccion = New System.Windows.Forms.TextBox
        Me.lblPorcentajeDeProteccion = New System.Windows.Forms.Label
        Me.NotifyIcon1 = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.gbDatosInsumo.SuspendLayout()
        Me.tcInsumo.SuspendLayout()
        Me.tpDatosInsumo.SuspendLayout()
        Me.SuspendLayout()
        '
        'gbDatosInsumo
        '
        Me.gbDatosInsumo.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbDatosInsumo.Controls.Add(Me.tcInsumo)
        Me.gbDatosInsumo.Location = New System.Drawing.Point(3, -2)
        Me.gbDatosInsumo.Name = "gbDatosInsumo"
        Me.gbDatosInsumo.Size = New System.Drawing.Size(590, 347)
        Me.gbDatosInsumo.TabIndex = 0
        Me.gbDatosInsumo.TabStop = False
        '
        'tcInsumo
        '
        Me.tcInsumo.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tcInsumo.Controls.Add(Me.tpDatosInsumo)
        Me.tcInsumo.Location = New System.Drawing.Point(6, 14)
        Me.tcInsumo.Name = "tcInsumo"
        Me.tcInsumo.SelectedIndex = 0
        Me.tcInsumo.Size = New System.Drawing.Size(578, 327)
        Me.tcInsumo.TabIndex = 16
        '
        'tpDatosInsumo
        '
        Me.tpDatosInsumo.Controls.Add(Me.lblCategoriaInsumo)
        Me.tpDatosInsumo.Controls.Add(Me.txtCategoria)
        Me.tpDatosInsumo.Controls.Add(Me.lblPrecioPieCubico)
        Me.tpDatosInsumo.Controls.Add(Me.lblLargo)
        Me.tpDatosInsumo.Controls.Add(Me.lblAncho)
        Me.tpDatosInsumo.Controls.Add(Me.lblEspesor)
        Me.tpDatosInsumo.Controls.Add(Me.txtPrecioPieCubico)
        Me.tpDatosInsumo.Controls.Add(Me.txtLargo)
        Me.tpDatosInsumo.Controls.Add(Me.txtAncho)
        Me.tpDatosInsumo.Controls.Add(Me.txtEspesor)
        Me.tpDatosInsumo.Controls.Add(Me.chkCubicar)
        Me.tpDatosInsumo.Controls.Add(Me.Label1)
        Me.tpDatosInsumo.Controls.Add(Me.lblNombreDelInsumo)
        Me.tpDatosInsumo.Controls.Add(Me.cmbTipoDeInsumo)
        Me.tpDatosInsumo.Controls.Add(Me.txtNombreDelInsumo)
        Me.tpDatosInsumo.Controls.Add(Me.btnCancelar)
        Me.tpDatosInsumo.Controls.Add(Me.lblUnidadDeMedida)
        Me.tpDatosInsumo.Controls.Add(Me.txtCostoParaTabulador)
        Me.tpDatosInsumo.Controls.Add(Me.txtUnidadDeMedida)
        Me.tpDatosInsumo.Controls.Add(Me.btnGuardar)
        Me.tpDatosInsumo.Controls.Add(Me.lblCostoSINIVA)
        Me.tpDatosInsumo.Controls.Add(Me.lblCostoParaTabulador)
        Me.tpDatosInsumo.Controls.Add(Me.txtCostoSINIVA)
        Me.tpDatosInsumo.Controls.Add(Me.txtPorcentajeDeProteccion)
        Me.tpDatosInsumo.Controls.Add(Me.lblPorcentajeDeProteccion)
        Me.tpDatosInsumo.Location = New System.Drawing.Point(4, 22)
        Me.tpDatosInsumo.Name = "tpDatosInsumo"
        Me.tpDatosInsumo.Padding = New System.Windows.Forms.Padding(3)
        Me.tpDatosInsumo.Size = New System.Drawing.Size(570, 301)
        Me.tpDatosInsumo.TabIndex = 0
        Me.tpDatosInsumo.Text = "Datos del Insumo"
        Me.tpDatosInsumo.UseVisualStyleBackColor = True
        '
        'lblCategoriaInsumo
        '
        Me.lblCategoriaInsumo.AutoSize = True
        Me.lblCategoriaInsumo.Location = New System.Drawing.Point(11, 77)
        Me.lblCategoriaInsumo.Name = "lblCategoriaInsumo"
        Me.lblCategoriaInsumo.Size = New System.Drawing.Size(57, 13)
        Me.lblCategoriaInsumo.TabIndex = 21
        Me.lblCategoriaInsumo.Text = "Categoría:"
        '
        'txtCategoria
        '
        Me.txtCategoria.Location = New System.Drawing.Point(129, 74)
        Me.txtCategoria.MaxLength = 20
        Me.txtCategoria.Name = "txtCategoria"
        Me.txtCategoria.Size = New System.Drawing.Size(426, 20)
        Me.txtCategoria.TabIndex = 2
        '
        'lblPrecioPieCubico
        '
        Me.lblPrecioPieCubico.AutoSize = True
        Me.lblPrecioPieCubico.Location = New System.Drawing.Point(349, 210)
        Me.lblPrecioPieCubico.Name = "lblPrecioPieCubico"
        Me.lblPrecioPieCubico.Size = New System.Drawing.Size(110, 13)
        Me.lblPrecioPieCubico.TabIndex = 19
        Me.lblPrecioPieCubico.Text = "Precio por pie cúbico:"
        Me.lblPrecioPieCubico.Visible = False
        '
        'lblLargo
        '
        Me.lblLargo.AutoSize = True
        Me.lblLargo.Location = New System.Drawing.Point(465, 181)
        Me.lblLargo.Name = "lblLargo"
        Me.lblLargo.Size = New System.Drawing.Size(37, 13)
        Me.lblLargo.TabIndex = 18
        Me.lblLargo.Text = "Largo:"
        Me.lblLargo.Visible = False
        '
        'lblAncho
        '
        Me.lblAncho.AutoSize = True
        Me.lblAncho.Location = New System.Drawing.Point(365, 181)
        Me.lblAncho.Name = "lblAncho"
        Me.lblAncho.Size = New System.Drawing.Size(41, 13)
        Me.lblAncho.TabIndex = 17
        Me.lblAncho.Text = "Ancho:"
        Me.lblAncho.Visible = False
        '
        'lblEspesor
        '
        Me.lblEspesor.AutoSize = True
        Me.lblEspesor.Location = New System.Drawing.Point(258, 181)
        Me.lblEspesor.Name = "lblEspesor"
        Me.lblEspesor.Size = New System.Drawing.Size(48, 13)
        Me.lblEspesor.TabIndex = 16
        Me.lblEspesor.Text = "Espesor:"
        Me.lblEspesor.Visible = False
        '
        'txtPrecioPieCubico
        '
        Me.txtPrecioPieCubico.Location = New System.Drawing.Point(468, 207)
        Me.txtPrecioPieCubico.MaxLength = 20
        Me.txtPrecioPieCubico.Name = "txtPrecioPieCubico"
        Me.txtPrecioPieCubico.Size = New System.Drawing.Size(87, 20)
        Me.txtPrecioPieCubico.TabIndex = 12
        Me.txtPrecioPieCubico.Text = "0.00"
        Me.txtPrecioPieCubico.Visible = False
        '
        'txtLargo
        '
        Me.txtLargo.Location = New System.Drawing.Point(508, 178)
        Me.txtLargo.MaxLength = 20
        Me.txtLargo.Name = "txtLargo"
        Me.txtLargo.Size = New System.Drawing.Size(47, 20)
        Me.txtLargo.TabIndex = 11
        Me.txtLargo.Text = "0.00"
        Me.txtLargo.Visible = False
        '
        'txtAncho
        '
        Me.txtAncho.Location = New System.Drawing.Point(412, 178)
        Me.txtAncho.MaxLength = 20
        Me.txtAncho.Name = "txtAncho"
        Me.txtAncho.Size = New System.Drawing.Size(47, 20)
        Me.txtAncho.TabIndex = 10
        Me.txtAncho.Text = "0.00"
        Me.txtAncho.Visible = False
        '
        'txtEspesor
        '
        Me.txtEspesor.Location = New System.Drawing.Point(312, 178)
        Me.txtEspesor.MaxLength = 20
        Me.txtEspesor.Name = "txtEspesor"
        Me.txtEspesor.Size = New System.Drawing.Size(47, 20)
        Me.txtEspesor.TabIndex = 9
        Me.txtEspesor.Text = "0.00"
        Me.txtEspesor.Visible = False
        '
        'chkCubicar
        '
        Me.chkCubicar.AutoSize = True
        Me.chkCubicar.Location = New System.Drawing.Point(372, 155)
        Me.chkCubicar.Name = "chkCubicar"
        Me.chkCubicar.Size = New System.Drawing.Size(183, 17)
        Me.chkCubicar.TabIndex = 8
        Me.chkCubicar.Text = "Calcular precio según cubicación"
        Me.chkCubicar.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(11, 103)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(86, 13)
        Me.Label1.TabIndex = 10
        Me.Label1.Text = "Tipo de Material:"
        '
        'lblNombreDelInsumo
        '
        Me.lblNombreDelInsumo.AutoSize = True
        Me.lblNombreDelInsumo.Location = New System.Drawing.Point(11, 16)
        Me.lblNombreDelInsumo.Name = "lblNombreDelInsumo"
        Me.lblNombreDelInsumo.Size = New System.Drawing.Size(101, 13)
        Me.lblNombreDelInsumo.TabIndex = 1
        Me.lblNombreDelInsumo.Text = "Nombre del Insumo:"
        '
        'cmbTipoDeInsumo
        '
        Me.cmbTipoDeInsumo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbTipoDeInsumo.FormattingEnabled = True
        Me.cmbTipoDeInsumo.Items.AddRange(New Object() {"MATERIALES", "MANO DE OBRA"})
        Me.cmbTipoDeInsumo.Location = New System.Drawing.Point(129, 100)
        Me.cmbTipoDeInsumo.Name = "cmbTipoDeInsumo"
        Me.cmbTipoDeInsumo.Size = New System.Drawing.Size(138, 21)
        Me.cmbTipoDeInsumo.TabIndex = 3
        '
        'txtNombreDelInsumo
        '
        Me.txtNombreDelInsumo.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtNombreDelInsumo.Location = New System.Drawing.Point(129, 13)
        Me.txtNombreDelInsumo.MaxLength = 300
        Me.txtNombreDelInsumo.Multiline = True
        Me.txtNombreDelInsumo.Name = "txtNombreDelInsumo"
        Me.txtNombreDelInsumo.Size = New System.Drawing.Size(426, 55)
        Me.txtNombreDelInsumo.TabIndex = 1
        '
        'btnCancelar
        '
        Me.btnCancelar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancelar.Image = Global.Oversight.My.Resources.Resources.cancel24x24
        Me.btnCancelar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCancelar.Location = New System.Drawing.Point(149, 252)
        Me.btnCancelar.Name = "btnCancelar"
        Me.btnCancelar.Size = New System.Drawing.Size(198, 34)
        Me.btnCancelar.TabIndex = 13
        Me.btnCancelar.Text = "&Cancelar / Regresar sin Cambios"
        Me.btnCancelar.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnCancelar.UseVisualStyleBackColor = True
        '
        'lblUnidadDeMedida
        '
        Me.lblUnidadDeMedida.AutoSize = True
        Me.lblUnidadDeMedida.Location = New System.Drawing.Point(11, 130)
        Me.lblUnidadDeMedida.Name = "lblUnidadDeMedida"
        Me.lblUnidadDeMedida.Size = New System.Drawing.Size(97, 13)
        Me.lblUnidadDeMedida.TabIndex = 2
        Me.lblUnidadDeMedida.Text = "Unidad de Medida:"
        '
        'txtCostoParaTabulador
        '
        Me.txtCostoParaTabulador.Enabled = False
        Me.txtCostoParaTabulador.Location = New System.Drawing.Point(129, 205)
        Me.txtCostoParaTabulador.MaxLength = 20
        Me.txtCostoParaTabulador.Name = "txtCostoParaTabulador"
        Me.txtCostoParaTabulador.Size = New System.Drawing.Size(100, 20)
        Me.txtCostoParaTabulador.TabIndex = 7
        '
        'txtUnidadDeMedida
        '
        Me.txtUnidadDeMedida.Location = New System.Drawing.Point(129, 127)
        Me.txtUnidadDeMedida.MaxLength = 100
        Me.txtUnidadDeMedida.Name = "txtUnidadDeMedida"
        Me.txtUnidadDeMedida.Size = New System.Drawing.Size(100, 20)
        Me.txtUnidadDeMedida.TabIndex = 4
        '
        'btnGuardar
        '
        Me.btnGuardar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnGuardar.Enabled = False
        Me.btnGuardar.Image = Global.Oversight.My.Resources.Resources.save24x24
        Me.btnGuardar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnGuardar.Location = New System.Drawing.Point(355, 252)
        Me.btnGuardar.Name = "btnGuardar"
        Me.btnGuardar.Size = New System.Drawing.Size(200, 34)
        Me.btnGuardar.TabIndex = 14
        Me.btnGuardar.Text = "&Guardar Cambios y Regresar"
        Me.btnGuardar.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnGuardar.UseVisualStyleBackColor = True
        '
        'lblCostoSINIVA
        '
        Me.lblCostoSINIVA.AutoSize = True
        Me.lblCostoSINIVA.Location = New System.Drawing.Point(11, 156)
        Me.lblCostoSINIVA.Name = "lblCostoSINIVA"
        Me.lblCostoSINIVA.Size = New System.Drawing.Size(78, 13)
        Me.lblCostoSINIVA.TabIndex = 4
        Me.lblCostoSINIVA.Text = "Costo SIN IVA:"
        '
        'lblCostoParaTabulador
        '
        Me.lblCostoParaTabulador.AutoSize = True
        Me.lblCostoParaTabulador.Location = New System.Drawing.Point(11, 208)
        Me.lblCostoParaTabulador.Name = "lblCostoParaTabulador"
        Me.lblCostoParaTabulador.Size = New System.Drawing.Size(112, 13)
        Me.lblCostoParaTabulador.TabIndex = 8
        Me.lblCostoParaTabulador.Text = "Costo para Tabulador:"
        '
        'txtCostoSINIVA
        '
        Me.txtCostoSINIVA.Location = New System.Drawing.Point(129, 153)
        Me.txtCostoSINIVA.MaxLength = 20
        Me.txtCostoSINIVA.Name = "txtCostoSINIVA"
        Me.txtCostoSINIVA.Size = New System.Drawing.Size(100, 20)
        Me.txtCostoSINIVA.TabIndex = 5
        '
        'txtPorcentajeDeProteccion
        '
        Me.txtPorcentajeDeProteccion.Location = New System.Drawing.Point(129, 179)
        Me.txtPorcentajeDeProteccion.MaxLength = 20
        Me.txtPorcentajeDeProteccion.Name = "txtPorcentajeDeProteccion"
        Me.txtPorcentajeDeProteccion.Size = New System.Drawing.Size(100, 20)
        Me.txtPorcentajeDeProteccion.TabIndex = 6
        '
        'lblPorcentajeDeProteccion
        '
        Me.lblPorcentajeDeProteccion.AutoSize = True
        Me.lblPorcentajeDeProteccion.Location = New System.Drawing.Point(11, 182)
        Me.lblPorcentajeDeProteccion.Name = "lblPorcentajeDeProteccion"
        Me.lblPorcentajeDeProteccion.Size = New System.Drawing.Size(87, 13)
        Me.lblPorcentajeDeProteccion.TabIndex = 6
        Me.lblPorcentajeDeProteccion.Text = "% de Protección:"
        '
        'NotifyIcon1
        '
        Me.NotifyIcon1.Icon = CType(resources.GetObject("NotifyIcon1.Icon"), System.Drawing.Icon)
        Me.NotifyIcon1.Text = "Oversight"
        '
        'AgregarInsumo
        '
        Me.AcceptButton = Me.btnGuardar
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCancelar
        Me.ClientSize = New System.Drawing.Size(598, 349)
        Me.Controls.Add(Me.gbDatosInsumo)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "AgregarInsumo"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Insumo"
        Me.gbDatosInsumo.ResumeLayout(False)
        Me.tcInsumo.ResumeLayout(False)
        Me.tpDatosInsumo.ResumeLayout(False)
        Me.tpDatosInsumo.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents gbDatosInsumo As System.Windows.Forms.GroupBox
    Friend WithEvents lblNombreDelInsumo As System.Windows.Forms.Label
    Friend WithEvents txtNombreDelInsumo As System.Windows.Forms.TextBox
    Friend WithEvents lblUnidadDeMedida As System.Windows.Forms.Label
    Friend WithEvents txtCostoParaTabulador As System.Windows.Forms.TextBox
    Friend WithEvents lblCostoParaTabulador As System.Windows.Forms.Label
    Friend WithEvents txtPorcentajeDeProteccion As System.Windows.Forms.TextBox
    Friend WithEvents lblPorcentajeDeProteccion As System.Windows.Forms.Label
    Friend WithEvents txtCostoSINIVA As System.Windows.Forms.TextBox
    Friend WithEvents lblCostoSINIVA As System.Windows.Forms.Label
    Friend WithEvents txtUnidadDeMedida As System.Windows.Forms.TextBox
    Friend WithEvents btnCancelar As System.Windows.Forms.Button
    Friend WithEvents btnGuardar As System.Windows.Forms.Button
    Friend WithEvents cmbTipoDeInsumo As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents tcInsumo As System.Windows.Forms.TabControl
    Friend WithEvents tpDatosInsumo As System.Windows.Forms.TabPage
    Friend WithEvents lblLargo As System.Windows.Forms.Label
    Friend WithEvents lblAncho As System.Windows.Forms.Label
    Friend WithEvents lblEspesor As System.Windows.Forms.Label
    Friend WithEvents txtPrecioPieCubico As System.Windows.Forms.TextBox
    Friend WithEvents txtLargo As System.Windows.Forms.TextBox
    Friend WithEvents txtAncho As System.Windows.Forms.TextBox
    Friend WithEvents txtEspesor As System.Windows.Forms.TextBox
    Friend WithEvents chkCubicar As System.Windows.Forms.CheckBox
    Friend WithEvents lblPrecioPieCubico As System.Windows.Forms.Label
    Friend WithEvents lblCategoriaInsumo As System.Windows.Forms.Label
    Friend WithEvents txtCategoria As System.Windows.Forms.TextBox
    Friend WithEvents NotifyIcon1 As System.Windows.Forms.NotifyIcon
End Class
