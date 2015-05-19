<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Reportes
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Reportes))
        Me.tcVentana = New System.Windows.Forms.TabControl
        Me.tpReportes = New System.Windows.Forms.TabPage
        Me.tcReportes = New System.Windows.Forms.TabControl
        Me.msSaveFileDialog = New System.Windows.Forms.SaveFileDialog
        Me.btnCancelar = New System.Windows.Forms.Button
        Me.NotifyIcon1 = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.tcVentana.SuspendLayout()
        Me.tpReportes.SuspendLayout()
        Me.SuspendLayout()
        '
        'tcVentana
        '
        Me.tcVentana.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tcVentana.Controls.Add(Me.tpReportes)
        Me.tcVentana.Location = New System.Drawing.Point(5, 7)
        Me.tcVentana.Name = "tcVentana"
        Me.tcVentana.SelectedIndex = 0
        Me.tcVentana.Size = New System.Drawing.Size(786, 422)
        Me.tcVentana.TabIndex = 18
        '
        'tpReportes
        '
        Me.tpReportes.Controls.Add(Me.tcReportes)
        Me.tpReportes.Location = New System.Drawing.Point(4, 22)
        Me.tpReportes.Name = "tpReportes"
        Me.tpReportes.Size = New System.Drawing.Size(778, 396)
        Me.tpReportes.TabIndex = 6
        Me.tpReportes.Text = "Reportes"
        Me.tpReportes.UseVisualStyleBackColor = True
        '
        'tcReportes
        '
        Me.tcReportes.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tcReportes.Location = New System.Drawing.Point(7, 8)
        Me.tcReportes.Name = "tcReportes"
        Me.tcReportes.SelectedIndex = 0
        Me.tcReportes.Size = New System.Drawing.Size(767, 379)
        Me.tcReportes.TabIndex = 71
        '
        'btnCancelar
        '
        Me.btnCancelar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancelar.Image = Global.Oversight.My.Resources.Resources.cancel24x24
        Me.btnCancelar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCancelar.Location = New System.Drawing.Point(669, 435)
        Me.btnCancelar.Name = "btnCancelar"
        Me.btnCancelar.Size = New System.Drawing.Size(122, 34)
        Me.btnCancelar.TabIndex = 19
        Me.btnCancelar.Text = "&Cerrar Ventana"
        Me.btnCancelar.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnCancelar.UseVisualStyleBackColor = True
        '
        'NotifyIcon1
        '
        Me.NotifyIcon1.Icon = CType(resources.GetObject("NotifyIcon1.Icon"), System.Drawing.Icon)
        Me.NotifyIcon1.Text = "Oversight"
        '
        'Reportes
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(799, 475)
        Me.Controls.Add(Me.btnCancelar)
        Me.Controls.Add(Me.tcVentana)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "Reportes"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Reportes"
        Me.tcVentana.ResumeLayout(False)
        Me.tpReportes.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents tcVentana As System.Windows.Forms.TabControl
    Friend WithEvents msSaveFileDialog As System.Windows.Forms.SaveFileDialog
    Friend WithEvents tpReportes As System.Windows.Forms.TabPage
    Friend WithEvents tcReportes As System.Windows.Forms.TabControl
    Friend WithEvents btnCancelar As System.Windows.Forms.Button
    Friend WithEvents NotifyIcon1 As System.Windows.Forms.NotifyIcon

End Class
