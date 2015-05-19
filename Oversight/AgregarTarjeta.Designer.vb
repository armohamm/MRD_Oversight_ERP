<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AgregarTarjeta
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(AgregarTarjeta))
        Me.dgvConceptosTarjeta = New System.Windows.Forms.DataGridView
        Me.gbDatosTarjeta = New System.Windows.Forms.GroupBox
        Me.tcTarjeta = New System.Windows.Forms.TabControl
        Me.tcAgregarTarjeta = New System.Windows.Forms.TabPage
        Me.btnCategorias = New System.Windows.Forms.Button
        Me.btnEliminarInsumo = New System.Windows.Forms.Button
        Me.btnInsertarInsumo = New System.Windows.Forms.Button
        Me.btnNuevoInsumo = New System.Windows.Forms.Button
        Me.lblAvailability = New System.Windows.Forms.Label
        Me.lblUltimaModificacion = New System.Windows.Forms.Label
        Me.lblPorcentajeIndirectos = New System.Windows.Forms.Label
        Me.txtPorcentajeIndirectos = New System.Windows.Forms.TextBox
        Me.txtNombreDeLaTarjeta = New System.Windows.Forms.TextBox
        Me.lblPorcentajeUtilidades = New System.Windows.Forms.Label
        Me.lblNombreDeLaTarjeta = New System.Windows.Forms.Label
        Me.txtPorcentajeUtilidades = New System.Windows.Forms.TextBox
        Me.txtCodigoDeLaTarjeta = New System.Windows.Forms.TextBox
        Me.lblPorcentajeIVA = New System.Windows.Forms.Label
        Me.lblCodigoDeLaTarjeta = New System.Windows.Forms.Label
        Me.txtUltimaModificacion = New System.Windows.Forms.TextBox
        Me.lblConceptosDeLaTarjeta = New System.Windows.Forms.Label
        Me.lblUtilidades = New System.Windows.Forms.Label
        Me.txtSubTotal = New System.Windows.Forms.TextBox
        Me.txtUtilidades = New System.Windows.Forms.TextBox
        Me.lblSubTotal = New System.Windows.Forms.Label
        Me.txtTotal = New System.Windows.Forms.TextBox
        Me.lblCategoriaDeLaTarjeta = New System.Windows.Forms.Label
        Me.lblTotal = New System.Windows.Forms.Label
        Me.cmbCategoriaDeLaTarjeta = New System.Windows.Forms.ComboBox
        Me.lblUnidadDeMedida = New System.Windows.Forms.Label
        Me.btnCancelar = New System.Windows.Forms.Button
        Me.txtUnidadDeMedida = New System.Windows.Forms.TextBox
        Me.btnGuardar = New System.Windows.Forms.Button
        Me.txtSuma = New System.Windows.Forms.TextBox
        Me.lblSuma = New System.Windows.Forms.Label
        Me.lblCostoDirecto = New System.Windows.Forms.Label
        Me.lblIndirectos = New System.Windows.Forms.Label
        Me.txtIVA = New System.Windows.Forms.TextBox
        Me.txtCostoDirecto = New System.Windows.Forms.TextBox
        Me.lblIVA = New System.Windows.Forms.Label
        Me.txtIndirectos = New System.Windows.Forms.TextBox
        Me.NotifyIcon1 = New System.Windows.Forms.NotifyIcon(Me.components)
        CType(Me.dgvConceptosTarjeta, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gbDatosTarjeta.SuspendLayout()
        Me.tcTarjeta.SuspendLayout()
        Me.tcAgregarTarjeta.SuspendLayout()
        Me.SuspendLayout()
        '
        'dgvConceptosTarjeta
        '
        Me.dgvConceptosTarjeta.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvConceptosTarjeta.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvConceptosTarjeta.Enabled = False
        Me.dgvConceptosTarjeta.Location = New System.Drawing.Point(15, 232)
        Me.dgvConceptosTarjeta.MultiSelect = False
        Me.dgvConceptosTarjeta.Name = "dgvConceptosTarjeta"
        Me.dgvConceptosTarjeta.RowHeadersVisible = False
        Me.dgvConceptosTarjeta.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me.dgvConceptosTarjeta.Size = New System.Drawing.Size(452, 158)
        Me.dgvConceptosTarjeta.TabIndex = 6
        '
        'gbDatosTarjeta
        '
        Me.gbDatosTarjeta.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbDatosTarjeta.Controls.Add(Me.tcTarjeta)
        Me.gbDatosTarjeta.Location = New System.Drawing.Point(5, 0)
        Me.gbDatosTarjeta.Name = "gbDatosTarjeta"
        Me.gbDatosTarjeta.Size = New System.Drawing.Size(539, 676)
        Me.gbDatosTarjeta.TabIndex = 1
        Me.gbDatosTarjeta.TabStop = False
        '
        'tcTarjeta
        '
        Me.tcTarjeta.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tcTarjeta.Controls.Add(Me.tcAgregarTarjeta)
        Me.tcTarjeta.Location = New System.Drawing.Point(6, 12)
        Me.tcTarjeta.Name = "tcTarjeta"
        Me.tcTarjeta.SelectedIndex = 0
        Me.tcTarjeta.Size = New System.Drawing.Size(527, 658)
        Me.tcTarjeta.TabIndex = 19
        '
        'tcAgregarTarjeta
        '
        Me.tcAgregarTarjeta.Controls.Add(Me.btnCategorias)
        Me.tcAgregarTarjeta.Controls.Add(Me.btnEliminarInsumo)
        Me.tcAgregarTarjeta.Controls.Add(Me.btnInsertarInsumo)
        Me.tcAgregarTarjeta.Controls.Add(Me.btnNuevoInsumo)
        Me.tcAgregarTarjeta.Controls.Add(Me.lblAvailability)
        Me.tcAgregarTarjeta.Controls.Add(Me.lblUltimaModificacion)
        Me.tcAgregarTarjeta.Controls.Add(Me.lblPorcentajeIndirectos)
        Me.tcAgregarTarjeta.Controls.Add(Me.dgvConceptosTarjeta)
        Me.tcAgregarTarjeta.Controls.Add(Me.txtPorcentajeIndirectos)
        Me.tcAgregarTarjeta.Controls.Add(Me.txtNombreDeLaTarjeta)
        Me.tcAgregarTarjeta.Controls.Add(Me.lblPorcentajeUtilidades)
        Me.tcAgregarTarjeta.Controls.Add(Me.lblNombreDeLaTarjeta)
        Me.tcAgregarTarjeta.Controls.Add(Me.txtPorcentajeUtilidades)
        Me.tcAgregarTarjeta.Controls.Add(Me.txtCodigoDeLaTarjeta)
        Me.tcAgregarTarjeta.Controls.Add(Me.lblPorcentajeIVA)
        Me.tcAgregarTarjeta.Controls.Add(Me.lblCodigoDeLaTarjeta)
        Me.tcAgregarTarjeta.Controls.Add(Me.txtUltimaModificacion)
        Me.tcAgregarTarjeta.Controls.Add(Me.lblConceptosDeLaTarjeta)
        Me.tcAgregarTarjeta.Controls.Add(Me.lblUtilidades)
        Me.tcAgregarTarjeta.Controls.Add(Me.txtSubTotal)
        Me.tcAgregarTarjeta.Controls.Add(Me.txtUtilidades)
        Me.tcAgregarTarjeta.Controls.Add(Me.lblSubTotal)
        Me.tcAgregarTarjeta.Controls.Add(Me.txtTotal)
        Me.tcAgregarTarjeta.Controls.Add(Me.lblCategoriaDeLaTarjeta)
        Me.tcAgregarTarjeta.Controls.Add(Me.lblTotal)
        Me.tcAgregarTarjeta.Controls.Add(Me.cmbCategoriaDeLaTarjeta)
        Me.tcAgregarTarjeta.Controls.Add(Me.lblUnidadDeMedida)
        Me.tcAgregarTarjeta.Controls.Add(Me.btnCancelar)
        Me.tcAgregarTarjeta.Controls.Add(Me.txtUnidadDeMedida)
        Me.tcAgregarTarjeta.Controls.Add(Me.btnGuardar)
        Me.tcAgregarTarjeta.Controls.Add(Me.txtSuma)
        Me.tcAgregarTarjeta.Controls.Add(Me.lblSuma)
        Me.tcAgregarTarjeta.Controls.Add(Me.lblCostoDirecto)
        Me.tcAgregarTarjeta.Controls.Add(Me.lblIndirectos)
        Me.tcAgregarTarjeta.Controls.Add(Me.txtIVA)
        Me.tcAgregarTarjeta.Controls.Add(Me.txtCostoDirecto)
        Me.tcAgregarTarjeta.Controls.Add(Me.lblIVA)
        Me.tcAgregarTarjeta.Controls.Add(Me.txtIndirectos)
        Me.tcAgregarTarjeta.Location = New System.Drawing.Point(4, 22)
        Me.tcAgregarTarjeta.Name = "tcAgregarTarjeta"
        Me.tcAgregarTarjeta.Size = New System.Drawing.Size(519, 632)
        Me.tcAgregarTarjeta.TabIndex = 2
        Me.tcAgregarTarjeta.Text = "Datos de la Tarjeta"
        Me.tcAgregarTarjeta.UseVisualStyleBackColor = True
        '
        'btnCategorias
        '
        Me.btnCategorias.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCategorias.Enabled = False
        Me.btnCategorias.Image = Global.Oversight.My.Resources.Resources.tag12x12
        Me.btnCategorias.Location = New System.Drawing.Point(412, 37)
        Me.btnCategorias.Name = "btnCategorias"
        Me.btnCategorias.Size = New System.Drawing.Size(28, 23)
        Me.btnCategorias.TabIndex = 74
        Me.btnCategorias.UseVisualStyleBackColor = True
        '
        'btnEliminarInsumo
        '
        Me.btnEliminarInsumo.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnEliminarInsumo.Enabled = False
        Me.btnEliminarInsumo.Image = Global.Oversight.My.Resources.Resources.delete12x12
        Me.btnEliminarInsumo.Location = New System.Drawing.Point(473, 290)
        Me.btnEliminarInsumo.Name = "btnEliminarInsumo"
        Me.btnEliminarInsumo.Size = New System.Drawing.Size(28, 23)
        Me.btnEliminarInsumo.TabIndex = 73
        Me.btnEliminarInsumo.UseVisualStyleBackColor = True
        '
        'btnInsertarInsumo
        '
        Me.btnInsertarInsumo.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnInsertarInsumo.Enabled = False
        Me.btnInsertarInsumo.Image = Global.Oversight.My.Resources.Resources.insertcard12x12
        Me.btnInsertarInsumo.Location = New System.Drawing.Point(473, 261)
        Me.btnInsertarInsumo.Name = "btnInsertarInsumo"
        Me.btnInsertarInsumo.Size = New System.Drawing.Size(28, 23)
        Me.btnInsertarInsumo.TabIndex = 72
        Me.btnInsertarInsumo.UseVisualStyleBackColor = True
        '
        'btnNuevoInsumo
        '
        Me.btnNuevoInsumo.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnNuevoInsumo.Enabled = False
        Me.btnNuevoInsumo.Image = Global.Oversight.My.Resources.Resources.newcard12x12
        Me.btnNuevoInsumo.Location = New System.Drawing.Point(473, 232)
        Me.btnNuevoInsumo.Name = "btnNuevoInsumo"
        Me.btnNuevoInsumo.Size = New System.Drawing.Size(28, 23)
        Me.btnNuevoInsumo.TabIndex = 71
        Me.btnNuevoInsumo.UseVisualStyleBackColor = True
        '
        'lblAvailability
        '
        Me.lblAvailability.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblAvailability.AutoSize = True
        Me.lblAvailability.ForeColor = System.Drawing.Color.ForestGreen
        Me.lblAvailability.Location = New System.Drawing.Point(413, 73)
        Me.lblAvailability.MaximumSize = New System.Drawing.Size(80, 13)
        Me.lblAvailability.MinimumSize = New System.Drawing.Size(56, 13)
        Me.lblAvailability.Name = "lblAvailability"
        Me.lblAvailability.Size = New System.Drawing.Size(56, 13)
        Me.lblAvailability.TabIndex = 70
        Me.lblAvailability.Text = "Disponible"
        Me.lblAvailability.Visible = False
        '
        'lblUltimaModificacion
        '
        Me.lblUltimaModificacion.AutoSize = True
        Me.lblUltimaModificacion.Location = New System.Drawing.Point(12, 16)
        Me.lblUltimaModificacion.Name = "lblUltimaModificacion"
        Me.lblUltimaModificacion.Size = New System.Drawing.Size(105, 13)
        Me.lblUltimaModificacion.TabIndex = 61
        Me.lblUltimaModificacion.Text = "Ultima Modificación: "
        '
        'lblPorcentajeIndirectos
        '
        Me.lblPorcentajeIndirectos.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblPorcentajeIndirectos.AutoSize = True
        Me.lblPorcentajeIndirectos.Location = New System.Drawing.Point(300, 424)
        Me.lblPorcentajeIndirectos.MaximumSize = New System.Drawing.Size(130, 0)
        Me.lblPorcentajeIndirectos.Name = "lblPorcentajeIndirectos"
        Me.lblPorcentajeIndirectos.Size = New System.Drawing.Size(15, 13)
        Me.lblPorcentajeIndirectos.TabIndex = 69
        Me.lblPorcentajeIndirectos.Text = "%"
        '
        'txtPorcentajeIndirectos
        '
        Me.txtPorcentajeIndirectos.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtPorcentajeIndirectos.Location = New System.Drawing.Point(251, 421)
        Me.txtPorcentajeIndirectos.MaxLength = 20
        Me.txtPorcentajeIndirectos.Name = "txtPorcentajeIndirectos"
        Me.txtPorcentajeIndirectos.Size = New System.Drawing.Size(43, 20)
        Me.txtPorcentajeIndirectos.TabIndex = 7
        Me.txtPorcentajeIndirectos.Text = "0"
        '
        'txtNombreDeLaTarjeta
        '
        Me.txtNombreDeLaTarjeta.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtNombreDeLaTarjeta.Location = New System.Drawing.Point(138, 124)
        Me.txtNombreDeLaTarjeta.MaxLength = 1000
        Me.txtNombreDeLaTarjeta.Multiline = True
        Me.txtNombreDeLaTarjeta.Name = "txtNombreDeLaTarjeta"
        Me.txtNombreDeLaTarjeta.Size = New System.Drawing.Size(363, 102)
        Me.txtNombreDeLaTarjeta.TabIndex = 5
        '
        'lblPorcentajeUtilidades
        '
        Me.lblPorcentajeUtilidades.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblPorcentajeUtilidades.AutoSize = True
        Me.lblPorcentajeUtilidades.Location = New System.Drawing.Point(300, 476)
        Me.lblPorcentajeUtilidades.MaximumSize = New System.Drawing.Size(130, 0)
        Me.lblPorcentajeUtilidades.Name = "lblPorcentajeUtilidades"
        Me.lblPorcentajeUtilidades.Size = New System.Drawing.Size(15, 13)
        Me.lblPorcentajeUtilidades.TabIndex = 67
        Me.lblPorcentajeUtilidades.Text = "%"
        '
        'lblNombreDeLaTarjeta
        '
        Me.lblNombreDeLaTarjeta.AutoSize = True
        Me.lblNombreDeLaTarjeta.Location = New System.Drawing.Point(13, 127)
        Me.lblNombreDeLaTarjeta.Name = "lblNombreDeLaTarjeta"
        Me.lblNombreDeLaTarjeta.Size = New System.Drawing.Size(109, 13)
        Me.lblNombreDeLaTarjeta.TabIndex = 21
        Me.lblNombreDeLaTarjeta.Text = "Nombre de la Tarjeta:"
        '
        'txtPorcentajeUtilidades
        '
        Me.txtPorcentajeUtilidades.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtPorcentajeUtilidades.Location = New System.Drawing.Point(251, 473)
        Me.txtPorcentajeUtilidades.MaxLength = 20
        Me.txtPorcentajeUtilidades.Name = "txtPorcentajeUtilidades"
        Me.txtPorcentajeUtilidades.Size = New System.Drawing.Size(43, 20)
        Me.txtPorcentajeUtilidades.TabIndex = 8
        Me.txtPorcentajeUtilidades.Text = "0"
        '
        'txtCodigoDeLaTarjeta
        '
        Me.txtCodigoDeLaTarjeta.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtCodigoDeLaTarjeta.Location = New System.Drawing.Point(138, 67)
        Me.txtCodigoDeLaTarjeta.MaxLength = 10
        Me.txtCodigoDeLaTarjeta.Name = "txtCodigoDeLaTarjeta"
        Me.txtCodigoDeLaTarjeta.Size = New System.Drawing.Size(268, 20)
        Me.txtCodigoDeLaTarjeta.TabIndex = 2
        '
        'lblPorcentajeIVA
        '
        Me.lblPorcentajeIVA.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblPorcentajeIVA.AutoSize = True
        Me.lblPorcentajeIVA.Location = New System.Drawing.Point(330, 528)
        Me.lblPorcentajeIVA.MaximumSize = New System.Drawing.Size(130, 0)
        Me.lblPorcentajeIVA.Name = "lblPorcentajeIVA"
        Me.lblPorcentajeIVA.Size = New System.Drawing.Size(41, 13)
        Me.lblPorcentajeIVA.TabIndex = 65
        Me.lblPorcentajeIVA.Text = "xx.xx %"
        '
        'lblCodigoDeLaTarjeta
        '
        Me.lblCodigoDeLaTarjeta.AutoSize = True
        Me.lblCodigoDeLaTarjeta.Location = New System.Drawing.Point(12, 70)
        Me.lblCodigoDeLaTarjeta.Name = "lblCodigoDeLaTarjeta"
        Me.lblCodigoDeLaTarjeta.Size = New System.Drawing.Size(105, 13)
        Me.lblCodigoDeLaTarjeta.TabIndex = 23
        Me.lblCodigoDeLaTarjeta.Text = "Código de la Tarjeta:"
        '
        'txtUltimaModificacion
        '
        Me.txtUltimaModificacion.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtUltimaModificacion.Enabled = False
        Me.txtUltimaModificacion.Location = New System.Drawing.Point(137, 13)
        Me.txtUltimaModificacion.Name = "txtUltimaModificacion"
        Me.txtUltimaModificacion.Size = New System.Drawing.Size(269, 20)
        Me.txtUltimaModificacion.TabIndex = 4
        Me.txtUltimaModificacion.TabStop = False
        '
        'lblConceptosDeLaTarjeta
        '
        Me.lblConceptosDeLaTarjeta.AutoSize = True
        Me.lblConceptosDeLaTarjeta.Location = New System.Drawing.Point(13, 200)
        Me.lblConceptosDeLaTarjeta.MaximumSize = New System.Drawing.Size(110, 0)
        Me.lblConceptosDeLaTarjeta.Name = "lblConceptosDeLaTarjeta"
        Me.lblConceptosDeLaTarjeta.Size = New System.Drawing.Size(87, 26)
        Me.lblConceptosDeLaTarjeta.TabIndex = 24
        Me.lblConceptosDeLaTarjeta.Text = "Conceptos de la Tarjeta:"
        '
        'lblUtilidades
        '
        Me.lblUtilidades.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblUtilidades.AutoSize = True
        Me.lblUtilidades.Location = New System.Drawing.Point(318, 476)
        Me.lblUtilidades.MaximumSize = New System.Drawing.Size(130, 0)
        Me.lblUtilidades.Name = "lblUtilidades"
        Me.lblUtilidades.Size = New System.Drawing.Size(53, 13)
        Me.lblUtilidades.TabIndex = 63
        Me.lblUtilidades.Text = "Utilidades"
        '
        'txtSubTotal
        '
        Me.txtSubTotal.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtSubTotal.Enabled = False
        Me.txtSubTotal.Location = New System.Drawing.Point(377, 499)
        Me.txtSubTotal.MaxLength = 20
        Me.txtSubTotal.Name = "txtSubTotal"
        Me.txtSubTotal.Size = New System.Drawing.Size(124, 20)
        Me.txtSubTotal.TabIndex = 13
        Me.txtSubTotal.TabStop = False
        Me.txtSubTotal.Text = "0.00"
        '
        'txtUtilidades
        '
        Me.txtUtilidades.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtUtilidades.Enabled = False
        Me.txtUtilidades.Location = New System.Drawing.Point(377, 473)
        Me.txtUtilidades.MaxLength = 20
        Me.txtUtilidades.Name = "txtUtilidades"
        Me.txtUtilidades.Size = New System.Drawing.Size(124, 20)
        Me.txtUtilidades.TabIndex = 12
        Me.txtUtilidades.TabStop = False
        Me.txtUtilidades.Text = "0.00"
        '
        'lblSubTotal
        '
        Me.lblSubTotal.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblSubTotal.AutoSize = True
        Me.lblSubTotal.Location = New System.Drawing.Point(321, 502)
        Me.lblSubTotal.MaximumSize = New System.Drawing.Size(130, 0)
        Me.lblSubTotal.Name = "lblSubTotal"
        Me.lblSubTotal.Size = New System.Drawing.Size(50, 13)
        Me.lblSubTotal.TabIndex = 43
        Me.lblSubTotal.Text = "SubTotal"
        '
        'txtTotal
        '
        Me.txtTotal.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtTotal.Enabled = False
        Me.txtTotal.Location = New System.Drawing.Point(377, 551)
        Me.txtTotal.MaxLength = 20
        Me.txtTotal.Name = "txtTotal"
        Me.txtTotal.Size = New System.Drawing.Size(124, 20)
        Me.txtTotal.TabIndex = 15
        Me.txtTotal.TabStop = False
        Me.txtTotal.Text = "0.00"
        '
        'lblCategoriaDeLaTarjeta
        '
        Me.lblCategoriaDeLaTarjeta.AutoSize = True
        Me.lblCategoriaDeLaTarjeta.Location = New System.Drawing.Point(12, 42)
        Me.lblCategoriaDeLaTarjeta.Name = "lblCategoriaDeLaTarjeta"
        Me.lblCategoriaDeLaTarjeta.Size = New System.Drawing.Size(119, 13)
        Me.lblCategoriaDeLaTarjeta.TabIndex = 60
        Me.lblCategoriaDeLaTarjeta.Text = "Categoría de la Tarjeta:"
        '
        'lblTotal
        '
        Me.lblTotal.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblTotal.AutoSize = True
        Me.lblTotal.Location = New System.Drawing.Point(340, 554)
        Me.lblTotal.MaximumSize = New System.Drawing.Size(130, 0)
        Me.lblTotal.Name = "lblTotal"
        Me.lblTotal.Size = New System.Drawing.Size(31, 13)
        Me.lblTotal.TabIndex = 45
        Me.lblTotal.Text = "Total"
        '
        'cmbCategoriaDeLaTarjeta
        '
        Me.cmbCategoriaDeLaTarjeta.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmbCategoriaDeLaTarjeta.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cmbCategoriaDeLaTarjeta.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbCategoriaDeLaTarjeta.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbCategoriaDeLaTarjeta.FormattingEnabled = True
        Me.cmbCategoriaDeLaTarjeta.Location = New System.Drawing.Point(137, 39)
        Me.cmbCategoriaDeLaTarjeta.Name = "cmbCategoriaDeLaTarjeta"
        Me.cmbCategoriaDeLaTarjeta.Size = New System.Drawing.Size(269, 21)
        Me.cmbCategoriaDeLaTarjeta.TabIndex = 1
        '
        'lblUnidadDeMedida
        '
        Me.lblUnidadDeMedida.AutoSize = True
        Me.lblUnidadDeMedida.Location = New System.Drawing.Point(12, 96)
        Me.lblUnidadDeMedida.MaximumSize = New System.Drawing.Size(130, 0)
        Me.lblUnidadDeMedida.Name = "lblUnidadDeMedida"
        Me.lblUnidadDeMedida.Size = New System.Drawing.Size(97, 13)
        Me.lblUnidadDeMedida.TabIndex = 46
        Me.lblUnidadDeMedida.Text = "Unidad de Medida:"
        '
        'btnCancelar
        '
        Me.btnCancelar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancelar.Image = Global.Oversight.My.Resources.Resources.cancel24x24
        Me.btnCancelar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCancelar.Location = New System.Drawing.Point(130, 586)
        Me.btnCancelar.Name = "btnCancelar"
        Me.btnCancelar.Size = New System.Drawing.Size(199, 34)
        Me.btnCancelar.TabIndex = 16
        Me.btnCancelar.Text = "&Cancelar / Regresar sin Cambios"
        Me.btnCancelar.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnCancelar.UseVisualStyleBackColor = True
        '
        'txtUnidadDeMedida
        '
        Me.txtUnidadDeMedida.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtUnidadDeMedida.Location = New System.Drawing.Point(137, 93)
        Me.txtUnidadDeMedida.MaxLength = 50
        Me.txtUnidadDeMedida.Name = "txtUnidadDeMedida"
        Me.txtUnidadDeMedida.Size = New System.Drawing.Size(269, 20)
        Me.txtUnidadDeMedida.TabIndex = 3
        '
        'btnGuardar
        '
        Me.btnGuardar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnGuardar.Enabled = False
        Me.btnGuardar.Image = Global.Oversight.My.Resources.Resources.save24x24
        Me.btnGuardar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnGuardar.Location = New System.Drawing.Point(333, 586)
        Me.btnGuardar.Name = "btnGuardar"
        Me.btnGuardar.Size = New System.Drawing.Size(168, 34)
        Me.btnGuardar.TabIndex = 17
        Me.btnGuardar.Text = "&Guardar Datos y Regresar"
        Me.btnGuardar.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnGuardar.UseVisualStyleBackColor = True
        '
        'txtSuma
        '
        Me.txtSuma.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtSuma.Enabled = False
        Me.txtSuma.Location = New System.Drawing.Point(377, 447)
        Me.txtSuma.MaxLength = 20
        Me.txtSuma.Name = "txtSuma"
        Me.txtSuma.Size = New System.Drawing.Size(124, 20)
        Me.txtSuma.TabIndex = 11
        Me.txtSuma.TabStop = False
        Me.txtSuma.Text = "0.00"
        '
        'lblSuma
        '
        Me.lblSuma.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblSuma.AutoSize = True
        Me.lblSuma.Location = New System.Drawing.Point(337, 450)
        Me.lblSuma.MaximumSize = New System.Drawing.Size(130, 0)
        Me.lblSuma.Name = "lblSuma"
        Me.lblSuma.Size = New System.Drawing.Size(34, 13)
        Me.lblSuma.TabIndex = 55
        Me.lblSuma.Text = "Suma"
        '
        'lblCostoDirecto
        '
        Me.lblCostoDirecto.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblCostoDirecto.AutoSize = True
        Me.lblCostoDirecto.Location = New System.Drawing.Point(300, 399)
        Me.lblCostoDirecto.MaximumSize = New System.Drawing.Size(130, 0)
        Me.lblCostoDirecto.Name = "lblCostoDirecto"
        Me.lblCostoDirecto.Size = New System.Drawing.Size(71, 13)
        Me.lblCostoDirecto.TabIndex = 49
        Me.lblCostoDirecto.Text = "Costo Directo"
        '
        'lblIndirectos
        '
        Me.lblIndirectos.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblIndirectos.AutoSize = True
        Me.lblIndirectos.Location = New System.Drawing.Point(318, 424)
        Me.lblIndirectos.MaximumSize = New System.Drawing.Size(130, 0)
        Me.lblIndirectos.Name = "lblIndirectos"
        Me.lblIndirectos.Size = New System.Drawing.Size(53, 13)
        Me.lblIndirectos.TabIndex = 54
        Me.lblIndirectos.Text = "Indirectos"
        '
        'txtIVA
        '
        Me.txtIVA.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtIVA.Enabled = False
        Me.txtIVA.Location = New System.Drawing.Point(377, 525)
        Me.txtIVA.MaxLength = 20
        Me.txtIVA.Name = "txtIVA"
        Me.txtIVA.Size = New System.Drawing.Size(124, 20)
        Me.txtIVA.TabIndex = 14
        Me.txtIVA.TabStop = False
        Me.txtIVA.Text = "0.00"
        '
        'txtCostoDirecto
        '
        Me.txtCostoDirecto.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtCostoDirecto.Enabled = False
        Me.txtCostoDirecto.Location = New System.Drawing.Point(377, 396)
        Me.txtCostoDirecto.MaxLength = 20
        Me.txtCostoDirecto.Name = "txtCostoDirecto"
        Me.txtCostoDirecto.Size = New System.Drawing.Size(124, 20)
        Me.txtCostoDirecto.TabIndex = 9
        Me.txtCostoDirecto.TabStop = False
        Me.txtCostoDirecto.Text = "0.00"
        '
        'lblIVA
        '
        Me.lblIVA.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblIVA.AutoSize = True
        Me.lblIVA.Location = New System.Drawing.Point(305, 528)
        Me.lblIVA.MaximumSize = New System.Drawing.Size(130, 0)
        Me.lblIVA.Name = "lblIVA"
        Me.lblIVA.Size = New System.Drawing.Size(24, 13)
        Me.lblIVA.TabIndex = 51
        Me.lblIVA.Text = "IVA"
        '
        'txtIndirectos
        '
        Me.txtIndirectos.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtIndirectos.Enabled = False
        Me.txtIndirectos.Location = New System.Drawing.Point(377, 421)
        Me.txtIndirectos.MaxLength = 20
        Me.txtIndirectos.Name = "txtIndirectos"
        Me.txtIndirectos.Size = New System.Drawing.Size(124, 20)
        Me.txtIndirectos.TabIndex = 10
        Me.txtIndirectos.TabStop = False
        Me.txtIndirectos.Text = "0.00"
        '
        'NotifyIcon1
        '
        Me.NotifyIcon1.Icon = CType(resources.GetObject("NotifyIcon1.Icon"), System.Drawing.Icon)
        Me.NotifyIcon1.Text = "Oversight"
        '
        'AgregarTarjeta
        '
        Me.AcceptButton = Me.btnGuardar
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCancelar
        Me.ClientSize = New System.Drawing.Size(549, 679)
        Me.Controls.Add(Me.gbDatosTarjeta)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "AgregarTarjeta"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Tarjeta"
        CType(Me.dgvConceptosTarjeta, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gbDatosTarjeta.ResumeLayout(False)
        Me.tcTarjeta.ResumeLayout(False)
        Me.tcAgregarTarjeta.ResumeLayout(False)
        Me.tcAgregarTarjeta.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents dgvConceptosTarjeta As System.Windows.Forms.DataGridView
    Friend WithEvents gbDatosTarjeta As System.Windows.Forms.GroupBox
    Friend WithEvents lblCodigoDeLaTarjeta As System.Windows.Forms.Label
    Friend WithEvents txtCodigoDeLaTarjeta As System.Windows.Forms.TextBox
    Friend WithEvents lblNombreDeLaTarjeta As System.Windows.Forms.Label
    Friend WithEvents txtNombreDeLaTarjeta As System.Windows.Forms.TextBox
    Friend WithEvents lblConceptosDeLaTarjeta As System.Windows.Forms.Label
    Friend WithEvents lblTotal As System.Windows.Forms.Label
    Friend WithEvents txtTotal As System.Windows.Forms.TextBox
    Friend WithEvents lblSubTotal As System.Windows.Forms.Label
    Friend WithEvents txtSubTotal As System.Windows.Forms.TextBox
    Friend WithEvents lblCostoDirecto As System.Windows.Forms.Label
    Friend WithEvents txtSuma As System.Windows.Forms.TextBox
    Friend WithEvents txtUnidadDeMedida As System.Windows.Forms.TextBox
    Friend WithEvents lblUnidadDeMedida As System.Windows.Forms.Label
    Friend WithEvents txtCostoDirecto As System.Windows.Forms.TextBox
    Friend WithEvents txtIndirectos As System.Windows.Forms.TextBox
    Friend WithEvents lblIVA As System.Windows.Forms.Label
    Friend WithEvents txtIVA As System.Windows.Forms.TextBox
    Friend WithEvents lblSuma As System.Windows.Forms.Label
    Friend WithEvents lblIndirectos As System.Windows.Forms.Label
    Friend WithEvents btnCancelar As System.Windows.Forms.Button
    Friend WithEvents btnGuardar As System.Windows.Forms.Button
    Friend WithEvents lblCategoriaDeLaTarjeta As System.Windows.Forms.Label
    Friend WithEvents cmbCategoriaDeLaTarjeta As System.Windows.Forms.ComboBox
    Friend WithEvents lblUltimaModificacion As System.Windows.Forms.Label
    Friend WithEvents txtUltimaModificacion As System.Windows.Forms.TextBox
    Friend WithEvents lblUtilidades As System.Windows.Forms.Label
    Friend WithEvents txtUtilidades As System.Windows.Forms.TextBox
    Friend WithEvents lblPorcentajeIVA As System.Windows.Forms.Label
    Friend WithEvents txtPorcentajeUtilidades As System.Windows.Forms.TextBox
    Friend WithEvents lblPorcentajeUtilidades As System.Windows.Forms.Label
    Friend WithEvents lblPorcentajeIndirectos As System.Windows.Forms.Label
    Friend WithEvents txtPorcentajeIndirectos As System.Windows.Forms.TextBox
    Friend WithEvents tcTarjeta As System.Windows.Forms.TabControl
    Friend WithEvents tcAgregarTarjeta As System.Windows.Forms.TabPage
    Friend WithEvents lblAvailability As System.Windows.Forms.Label
    Friend WithEvents btnEliminarInsumo As System.Windows.Forms.Button
    Friend WithEvents btnInsertarInsumo As System.Windows.Forms.Button
    Friend WithEvents btnNuevoInsumo As System.Windows.Forms.Button
    Friend WithEvents btnCategorias As System.Windows.Forms.Button
    Friend WithEvents NotifyIcon1 As System.Windows.Forms.NotifyIcon
End Class
