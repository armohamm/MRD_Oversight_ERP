<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AgregarProyectoCambiaIndirectosYUtilidad
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(AgregarProyectoCambiaIndirectosYUtilidad))
        Me.gbPorcentajes = New System.Windows.Forms.GroupBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.txtPorcentajeUtilidad = New System.Windows.Forms.TextBox
        Me.btnGuardar = New System.Windows.Forms.Button
        Me.btnCancelar = New System.Windows.Forms.Button
        Me.lblBuscar = New System.Windows.Forms.Label
        Me.txtPorcentajeIndirectos = New System.Windows.Forms.TextBox
        Me.NotifyIcon1 = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.gbPorcentajes.SuspendLayout()
        Me.SuspendLayout()
        '
        'gbPorcentajes
        '
        Me.gbPorcentajes.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbPorcentajes.Controls.Add(Me.Label4)
        Me.gbPorcentajes.Controls.Add(Me.Label3)
        Me.gbPorcentajes.Controls.Add(Me.Label2)
        Me.gbPorcentajes.Controls.Add(Me.Label1)
        Me.gbPorcentajes.Controls.Add(Me.txtPorcentajeUtilidad)
        Me.gbPorcentajes.Controls.Add(Me.btnGuardar)
        Me.gbPorcentajes.Controls.Add(Me.btnCancelar)
        Me.gbPorcentajes.Controls.Add(Me.lblBuscar)
        Me.gbPorcentajes.Controls.Add(Me.txtPorcentajeIndirectos)
        Me.gbPorcentajes.Location = New System.Drawing.Point(7, 2)
        Me.gbPorcentajes.Name = "gbPorcentajes"
        Me.gbPorcentajes.Size = New System.Drawing.Size(310, 167)
        Me.gbPorcentajes.TabIndex = 29
        Me.gbPorcentajes.TabStop = False
        '
        'Label4
        '
        Me.Label4.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(257, 81)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(15, 13)
        Me.Label4.TabIndex = 35
        Me.Label4.Text = "%"
        '
        'Label3
        '
        Me.Label3.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(257, 55)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(15, 13)
        Me.Label3.TabIndex = 34
        Me.Label3.Text = "%"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(8, 16)
        Me.Label2.MaximumSize = New System.Drawing.Size(300, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(286, 26)
        Me.Label2.TabIndex = 33
        Me.Label2.Text = "Cuidado! Cambiar estos porcentajes afectará a TODAS las Tarjetas del Proyecto."
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(8, 81)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(111, 13)
        Me.Label1.TabIndex = 32
        Me.Label1.Text = "Porcentaje de Utilidad"
        '
        'txtPorcentajeUtilidad
        '
        Me.txtPorcentajeUtilidad.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtPorcentajeUtilidad.Location = New System.Drawing.Point(136, 78)
        Me.txtPorcentajeUtilidad.MaxLength = 20
        Me.txtPorcentajeUtilidad.Name = "txtPorcentajeUtilidad"
        Me.txtPorcentajeUtilidad.Size = New System.Drawing.Size(115, 20)
        Me.txtPorcentajeUtilidad.TabIndex = 2
        '
        'btnGuardar
        '
        Me.btnGuardar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnGuardar.Enabled = False
        Me.btnGuardar.Image = Global.Oversight.My.Resources.Resources.open24x24
        Me.btnGuardar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnGuardar.Location = New System.Drawing.Point(177, 124)
        Me.btnGuardar.Name = "btnGuardar"
        Me.btnGuardar.Size = New System.Drawing.Size(127, 34)
        Me.btnGuardar.TabIndex = 4
        Me.btnGuardar.Text = "&Guardar Cambios"
        Me.btnGuardar.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnGuardar.UseVisualStyleBackColor = True
        '
        'btnCancelar
        '
        Me.btnCancelar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancelar.Image = Global.Oversight.My.Resources.Resources.cancel24x24
        Me.btnCancelar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCancelar.Location = New System.Drawing.Point(79, 124)
        Me.btnCancelar.Name = "btnCancelar"
        Me.btnCancelar.Size = New System.Drawing.Size(89, 34)
        Me.btnCancelar.TabIndex = 3
        Me.btnCancelar.Text = "&Cancelar"
        Me.btnCancelar.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnCancelar.UseVisualStyleBackColor = True
        '
        'lblBuscar
        '
        Me.lblBuscar.AutoSize = True
        Me.lblBuscar.Location = New System.Drawing.Point(8, 55)
        Me.lblBuscar.Name = "lblBuscar"
        Me.lblBuscar.Size = New System.Drawing.Size(122, 13)
        Me.lblBuscar.TabIndex = 25
        Me.lblBuscar.Text = "Porcentaje de Indirectos"
        '
        'txtPorcentajeIndirectos
        '
        Me.txtPorcentajeIndirectos.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtPorcentajeIndirectos.Location = New System.Drawing.Point(136, 52)
        Me.txtPorcentajeIndirectos.MaxLength = 20
        Me.txtPorcentajeIndirectos.Name = "txtPorcentajeIndirectos"
        Me.txtPorcentajeIndirectos.Size = New System.Drawing.Size(115, 20)
        Me.txtPorcentajeIndirectos.TabIndex = 1
        '
        'NotifyIcon1
        '
        Me.NotifyIcon1.Icon = CType(resources.GetObject("NotifyIcon1.Icon"), System.Drawing.Icon)
        Me.NotifyIcon1.Text = "Oversight"
        '
        'AgregarProyectoCambiaIndirectosYUtilidad
        '
        Me.AcceptButton = Me.btnGuardar
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCancelar
        Me.ClientSize = New System.Drawing.Size(323, 173)
        Me.Controls.Add(Me.gbPorcentajes)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Name = "AgregarProyectoCambiaIndirectosYUtilidad"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Porcentajes de Indirectos y Utilidades"
        Me.gbPorcentajes.ResumeLayout(False)
        Me.gbPorcentajes.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents gbPorcentajes As System.Windows.Forms.GroupBox
    Friend WithEvents btnGuardar As System.Windows.Forms.Button
    Friend WithEvents btnCancelar As System.Windows.Forms.Button
    Friend WithEvents lblBuscar As System.Windows.Forms.Label
    Friend WithEvents txtPorcentajeIndirectos As System.Windows.Forms.TextBox
    Friend WithEvents txtPorcentajeUtilidad As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents NotifyIcon1 As System.Windows.Forms.NotifyIcon
End Class
