<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AgregarUnidad
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(AgregarUnidad))
        Me.gbUnidad = New System.Windows.Forms.GroupBox
        Me.lblAvailability = New System.Windows.Forms.Label
        Me.lblExample2 = New System.Windows.Forms.Label
        Me.lblExample1 = New System.Windows.Forms.Label
        Me.lblUnidadDestino = New System.Windows.Forms.Label
        Me.txtUnidadDestino = New System.Windows.Forms.TextBox
        Me.lblUnidadOrigen = New System.Windows.Forms.Label
        Me.txtUnidadOrigen = New System.Windows.Forms.TextBox
        Me.btnGuardar = New System.Windows.Forms.Button
        Me.btnCancelar = New System.Windows.Forms.Button
        Me.lblFactorConversion = New System.Windows.Forms.Label
        Me.txtFactorConversion = New System.Windows.Forms.TextBox
        Me.NotifyIcon1 = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.gbUnidad.SuspendLayout()
        Me.SuspendLayout()
        '
        'gbUnidad
        '
        Me.gbUnidad.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbUnidad.Controls.Add(Me.lblAvailability)
        Me.gbUnidad.Controls.Add(Me.lblExample2)
        Me.gbUnidad.Controls.Add(Me.lblExample1)
        Me.gbUnidad.Controls.Add(Me.lblUnidadDestino)
        Me.gbUnidad.Controls.Add(Me.txtUnidadDestino)
        Me.gbUnidad.Controls.Add(Me.lblUnidadOrigen)
        Me.gbUnidad.Controls.Add(Me.txtUnidadOrigen)
        Me.gbUnidad.Controls.Add(Me.btnGuardar)
        Me.gbUnidad.Controls.Add(Me.btnCancelar)
        Me.gbUnidad.Controls.Add(Me.lblFactorConversion)
        Me.gbUnidad.Controls.Add(Me.txtFactorConversion)
        Me.gbUnidad.Location = New System.Drawing.Point(3, -2)
        Me.gbUnidad.Name = "gbUnidad"
        Me.gbUnidad.Size = New System.Drawing.Size(327, 198)
        Me.gbUnidad.TabIndex = 57
        Me.gbUnidad.TabStop = False
        '
        'lblAvailability
        '
        Me.lblAvailability.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblAvailability.AutoSize = True
        Me.lblAvailability.ForeColor = System.Drawing.Color.ForestGreen
        Me.lblAvailability.Location = New System.Drawing.Point(257, 32)
        Me.lblAvailability.MaximumSize = New System.Drawing.Size(60, 0)
        Me.lblAvailability.MinimumSize = New System.Drawing.Size(60, 13)
        Me.lblAvailability.Name = "lblAvailability"
        Me.lblAvailability.Size = New System.Drawing.Size(60, 26)
        Me.lblAvailability.TabIndex = 71
        Me.lblAvailability.Text = "Conversión Disponible"
        Me.lblAvailability.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.lblAvailability.Visible = False
        '
        'lblExample2
        '
        Me.lblExample2.AutoSize = True
        Me.lblExample2.Location = New System.Drawing.Point(114, 117)
        Me.lblExample2.Name = "lblExample2"
        Me.lblExample2.Size = New System.Drawing.Size(150, 13)
        Me.lblExample2.TabIndex = 65
        Me.lblExample2.Text = "x cantidad de Unidad Destino)"
        '
        'lblExample1
        '
        Me.lblExample1.AutoSize = True
        Me.lblExample1.Location = New System.Drawing.Point(111, 100)
        Me.lblExample1.Name = "lblExample1"
        Me.lblExample1.Size = New System.Drawing.Size(146, 13)
        Me.lblExample1.TabIndex = 64
        Me.lblExample1.Text = "(1 unidad de Unidad Origen ="
        '
        'lblUnidadDestino
        '
        Me.lblUnidadDestino.AutoSize = True
        Me.lblUnidadDestino.Location = New System.Drawing.Point(8, 48)
        Me.lblUnidadDestino.Name = "lblUnidadDestino"
        Me.lblUnidadDestino.Size = New System.Drawing.Size(80, 13)
        Me.lblUnidadDestino.TabIndex = 63
        Me.lblUnidadDestino.Text = "Unidad Destino"
        '
        'txtUnidadDestino
        '
        Me.txtUnidadDestino.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtUnidadDestino.Location = New System.Drawing.Point(122, 45)
        Me.txtUnidadDestino.MaxLength = 150
        Me.txtUnidadDestino.Name = "txtUnidadDestino"
        Me.txtUnidadDestino.Size = New System.Drawing.Size(121, 20)
        Me.txtUnidadDestino.TabIndex = 2
        '
        'lblUnidadOrigen
        '
        Me.lblUnidadOrigen.AutoSize = True
        Me.lblUnidadOrigen.Location = New System.Drawing.Point(8, 22)
        Me.lblUnidadOrigen.Name = "lblUnidadOrigen"
        Me.lblUnidadOrigen.Size = New System.Drawing.Size(75, 13)
        Me.lblUnidadOrigen.TabIndex = 61
        Me.lblUnidadOrigen.Text = "Unidad Origen"
        '
        'txtUnidadOrigen
        '
        Me.txtUnidadOrigen.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtUnidadOrigen.Location = New System.Drawing.Point(122, 19)
        Me.txtUnidadOrigen.MaxLength = 150
        Me.txtUnidadOrigen.Name = "txtUnidadOrigen"
        Me.txtUnidadOrigen.Size = New System.Drawing.Size(121, 20)
        Me.txtUnidadOrigen.TabIndex = 1
        '
        'btnGuardar
        '
        Me.btnGuardar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnGuardar.Enabled = False
        Me.btnGuardar.Image = Global.Oversight.My.Resources.Resources.unit24x24
        Me.btnGuardar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnGuardar.Location = New System.Drawing.Point(193, 151)
        Me.btnGuardar.Name = "btnGuardar"
        Me.btnGuardar.Size = New System.Drawing.Size(120, 34)
        Me.btnGuardar.TabIndex = 5
        Me.btnGuardar.Text = "&Guardar Unidad"
        Me.btnGuardar.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnGuardar.UseVisualStyleBackColor = True
        '
        'btnCancelar
        '
        Me.btnCancelar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancelar.Image = Global.Oversight.My.Resources.Resources.cancel24x24
        Me.btnCancelar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCancelar.Location = New System.Drawing.Point(86, 151)
        Me.btnCancelar.Name = "btnCancelar"
        Me.btnCancelar.Size = New System.Drawing.Size(101, 34)
        Me.btnCancelar.TabIndex = 4
        Me.btnCancelar.Text = "&Cancelar"
        Me.btnCancelar.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnCancelar.UseVisualStyleBackColor = True
        '
        'lblFactorConversion
        '
        Me.lblFactorConversion.AutoSize = True
        Me.lblFactorConversion.Location = New System.Drawing.Point(8, 74)
        Me.lblFactorConversion.Name = "lblFactorConversion"
        Me.lblFactorConversion.Size = New System.Drawing.Size(108, 13)
        Me.lblFactorConversion.TabIndex = 51
        Me.lblFactorConversion.Text = "Factor de Conversión"
        '
        'txtFactorConversion
        '
        Me.txtFactorConversion.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtFactorConversion.Location = New System.Drawing.Point(122, 71)
        Me.txtFactorConversion.MaxLength = 20
        Me.txtFactorConversion.Name = "txtFactorConversion"
        Me.txtFactorConversion.Size = New System.Drawing.Size(121, 20)
        Me.txtFactorConversion.TabIndex = 3
        '
        'NotifyIcon1
        '
        Me.NotifyIcon1.Icon = CType(resources.GetObject("NotifyIcon1.Icon"), System.Drawing.Icon)
        Me.NotifyIcon1.Text = "Oversight"
        '
        'AgregarUnidad
        '
        Me.AcceptButton = Me.btnGuardar
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCancelar
        Me.ClientSize = New System.Drawing.Size(335, 200)
        Me.Controls.Add(Me.gbUnidad)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "AgregarUnidad"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Unidad"
        Me.gbUnidad.ResumeLayout(False)
        Me.gbUnidad.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents gbUnidad As System.Windows.Forms.GroupBox
    Friend WithEvents lblFactorConversion As System.Windows.Forms.Label
    Friend WithEvents txtFactorConversion As System.Windows.Forms.TextBox
    Friend WithEvents btnGuardar As System.Windows.Forms.Button
    Friend WithEvents btnCancelar As System.Windows.Forms.Button
    Friend WithEvents lblExample2 As System.Windows.Forms.Label
    Friend WithEvents lblExample1 As System.Windows.Forms.Label
    Friend WithEvents lblUnidadDestino As System.Windows.Forms.Label
    Friend WithEvents txtUnidadDestino As System.Windows.Forms.TextBox
    Friend WithEvents lblUnidadOrigen As System.Windows.Forms.Label
    Friend WithEvents txtUnidadOrigen As System.Windows.Forms.TextBox
    Friend WithEvents lblAvailability As System.Windows.Forms.Label
    Friend WithEvents NotifyIcon1 As System.Windows.Forms.NotifyIcon
End Class
