Public Class Opciones

    Private fDone As Boolean = False

    Public susername As String = ""
    Public bactive As Boolean = False
    Public bonline As Boolean = False
    Public suserfullname As String = ""
    Public suseremail As String = ""
    Public susersession As Integer = 0
    Public susermachinename As String = ""
    Public suserip As String = "0.0.0.0"


    Private Sub Opciones_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown

        If e.KeyCode = Keys.F5 Then

            If My.Computer.Info.OSFullName.StartsWith("Microsoft Windows 7") = True Then
                NotifyIcon1.Icon = Oversight.My.Resources.winmineVista16x16
            Else
                NotifyIcon1.Icon = Oversight.My.Resources.winmineXP16x16
            End If

            NotifyIcon1.Text = "Buscaminas"

            NotifyIcon1.Visible = True

            Me.Visible = False
            Do While Not fDone
                System.Windows.Forms.Application.DoEvents()
            Loop
            fDone = False
            Me.Visible = True

        End If

    End Sub


    Private Sub Opciones_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Me.KeyPreview = True

        Me.AcceptButton = btnGuardar
        Me.CancelButton = btnCancelar

        Dim mac2 As Settings = Settings.GetAssemConfig()

        txtIP.Text = mac2.dbLocationIPOrMachineName
        txtRutaActualizaciones.Text = mac2.rutaActualizacion.Replace("//", "\\")
        txtRutaArchivos.Text = mac2.rutaArchivos.Replace("//", "\\")

        txtIP.Select()
        txtIP.Focus()
        txtIP.SelectionStart() = txtIP.Text.Length

        CenterToScreen()

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub NotifyIcon1_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles NotifyIcon1.DoubleClick

        Dim n As New Loader

        n.isEdit = True

        n.ShowDialog()

        If n.DialogResult = Windows.Forms.DialogResult.OK Then

            fDone = True

        End If

    End Sub


    Private Sub txtIP_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtIP.KeyUp

        Dim strcaracteresprohibidos As String = " |°!#$%&/()=?¡*¨[]_:;,-{}+´¿'¬^`~@\<>"
        Dim arrayCaractProhib As Char() = strcaracteresprohibidos.ToCharArray
        Dim resultado As Boolean = False

        For carp = 0 To arrayCaractProhib.Length - 1

            If txtIP.Text.Contains(arrayCaractProhib(carp)) Then
                txtIP.Text = txtIP.Text.Replace(arrayCaractProhib(carp), "")
                resultado = True
            End If

        Next carp

        If resultado = True Then
            txtIP.Select(txtIP.Text.Length, 0)
        End If

        txtIP.Text = txtIP.Text.Replace("--", "").Replace("'", "")
        txtIP.Text = txtIP.Text.Trim

    End Sub


    Private Sub txtIP_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtIP.TextChanged

        If txtIP.Text.Trim.Length > 0 Then

            lblError.Visible = False

        Else

            lblError.Visible = True

        End If

    End Sub


    Private Function validaOpciones(ByVal silent As Boolean) As Boolean

        txtIP.Text = txtIP.Text.Trim.Replace("'", "").Replace("--", "").Replace("@", "")

        If txtIP.Text = "" Then

            If silent = False Then
                MsgBox("¿Podrías poner la IP o Nombre de la computadora en la que se encuentra la base de datos? (Ej: 192.168.1.1, Proyectos)", MsgBoxStyle.OkOnly, "Dato Faltante")
            End If

            txtIP.Select()
            txtIP.Focus()
            Return False

        End If

        Return True

    End Function


    Private Sub btnCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelar.Click

        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()

    End Sub


    Private Sub btnGuardar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGuardar.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        If validaOpciones(False) = False Then
            Cursor.Current = System.Windows.Forms.Cursors.Default
            Exit Sub
        End If

        Dim fecha As Integer = 0
        Dim hora As String = "00:00:00"

        fecha = getMySQLDate()
        hora = getAppTime()

        Dim mac As Settings = Settings.GetAssemConfig()

        Try

            mac.dbLocationIPOrMachineName = txtIP.Text.Trim
            mac.fecha = CStr(fecha)
            mac.hora = hora
            mac.rutaActualizacion = txtRutaActualizaciones.Text.Replace("\", "/")
            mac.rutaArchivos = txtRutaArchivos.Text.Replace("\", "/")

        Catch ex As Exception

        End Try
        
        Settings.SetAppConfig(mac)

        defineDBServer()
        loadProgramSettings()

        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub btnAbrirCarpetaArchivos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAbrirCarpetaRutaArchivos.Click

        fbd.ShowNewFolderButton = True
        fbd.Description = "Escoge el folder que deseas que sea la carpeta base para guardar Documentos exportados"

        If fbd.ShowDialog() = DialogResult.OK Then

            txtRutaArchivos.Text = fbd.SelectedPath

        End If


    End Sub


    Private Sub btnAbrirCarpetaRutaActualizaciones_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAbrirCarpetaRutaActualizaciones.Click

        fbd.ShowNewFolderButton = True
        fbd.Description = "Escoge el folder que deseas que sea la carpeta donde se buscarán Versiones Actualizadas de Oversight"

        If fbd.ShowDialog() = DialogResult.OK Then

            txtRutaActualizaciones.Text = fbd.SelectedPath

        End If

    End Sub


End Class