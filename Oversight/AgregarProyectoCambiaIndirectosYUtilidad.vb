Public Class AgregarProyectoCambiaIndirectosYUtilidad

    Private fDone As Boolean = False

    Public susername As String = ""
    Public bactive As Boolean = False
    Public bonline As Boolean = False
    Public suserfullname As String = ""
    Public suseremail As String = ""
    Public susersession As Integer = 0
    Public susermachinename As String = ""
    Public suserip As String = "0.0.0.0"

    Public isBase As Boolean = False
    Public IsModel As Boolean = False

    Public iprojectid As Integer = 0

    Private openPermission As Boolean = False


    Private Sub checkForKickoutsAndTimedOuts()

        Dim queryMySession As String = ""
        Dim dsMySession As DataSet

        queryMySession = "SELECT * FROM sessions s WHERE s.susername = '" & susername & "' AND s.susersession = '" & susersession & "' ORDER BY s.ilogindate DESC, s.slogintime DESC LIMIT 1 "

        dsMySession = getSQLQueryAsDataset(0, queryMySession)

        If dsMySession.Tables(0).Rows.Count > 0 Then

            If dsMySession.Tables(0).Rows(0).Item("btimedout") = "1" Then

                MsgBox("Tu sesión ha expirado. Es necesario que entres de nuevo al sistema con tu usuario y contraseña", MsgBoxStyle.Critical, "Sesión expirada")

                susername = ""
                bactive = False
                bonline = False
                suserfullname = ""
                suseremail = ""
                susersession = 0
                susermachinename = ""
                suserip = "0.0.0.0"

                Dim l As New Login

                l.isEdit = True

                l.ShowDialog(Me)

                If l.DialogResult <> Windows.Forms.DialogResult.OK Then

                    MsgBox("Cerrando Aplicación SIN Guardar...", MsgBoxStyle.Critical, "Intento Fallido")
                    System.Environment.Exit(0)

                End If

            End If

            If dsMySession.Tables(0).Rows(0).Item("bkickedout") = "1" Then

                MsgBox("Has sido sacado del sistema. Para continuar es necesario que entres de nuevo al sistema con tu usuario y contraseña", MsgBoxStyle.Critical, "Logged out")

                susername = ""
                bactive = False
                bonline = False
                suserfullname = ""
                suseremail = ""
                susersession = 0
                susermachinename = ""
                suserip = "0.0.0.0"

                Dim l As New Login

                l.isEdit = True

                l.ShowDialog(Me)

                If l.DialogResult <> Windows.Forms.DialogResult.OK Then

                    MsgBox("Cerrando Aplicación SIN Guardar...", MsgBoxStyle.Critical, "Intento Fallido")
                    System.Environment.Exit(0)

                End If

            End If

        End If

    End Sub


    Private Sub setControlsByPermissions(ByVal windowname As String, ByVal username As String)

        'Check for specific permissions on every window, but only for that unique window permissions, not the entire list!!

        Dim dsPermissions As DataSet

        Dim permission As String

        Dim viewPermission As Boolean = False

        dsPermissions = getSQLQueryAsDataset(0, "SELECT * FROM userpermissions WHERE susername = '" & username & "' AND swindowname = '" & windowname & "'")

        For j = 0 To dsPermissions.Tables(0).Rows.Count - 1

            Try

                permission = dsPermissions.Tables(0).Rows(j).Item("spermission")

                If permission = "Ver" Then
                    viewPermission = True
                End If

                If permission = "Modificar" Then
                    btnGuardar.Enabled = True
                End If

            Catch ex As Exception

            End Try

            permission = ""

        Next j


        If viewPermission = False Then

            Dim fecha As Integer = 0
            Dim hora As String = "00:00:00"

            fecha = getMySQLDate()
            hora = getAppTime()

            executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Acceso denegado a la ventana de Cambiar Indirectos e Utilidad', 'OK')")

            Dim dsUsuariosSysAdmin As DataSet

            dsUsuariosSysAdmin = getSQLQueryAsDataset(0, "SELECT susername FROM userspecialattributes WHERE bsysadmin = 1")

            If dsUsuariosSysAdmin.Tables(0).Rows.Count > 0 Then

                For i = 0 To dsUsuariosSysAdmin.Tables(0).Rows.Count - 1
                    executeSQLCommand(0, "INSERT INTO messages (susername, susersession, smessage, bread, imessagedatetime, smessagecreatorusername, iupdatedatetime, supdateusername) VALUES ('" & dsUsuariosSysAdmin.Tables(0).Rows(i).Item(0) & "', 0, 'Un usuario ha intentado propasar sus permisos. ¿Podrías revisar?', 0, '" & convertYYYYMMDDtoYYYYhyphenMMhyphenDD(fecha) & " " & hora & "', 'SYSTEM', '" & convertYYYYMMDDtoYYYYhyphenMMhyphenDD(fecha) & " " & hora & "', 'SYSTEM')")
                Next i

            End If

            MsgBox("No tienes los permisos necesarios para abrir esta Ventana. Este intento ha sido notificado al administrador.", MsgBoxStyle.Exclamation, "Access Denied")
            Me.DialogResult = Windows.Forms.DialogResult.Cancel
            Me.Close()

        End If

    End Sub


    Private Sub AgregarProyectoCambiaIndirectosYUtilidad_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        verifySuspiciousData()

        executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Cerró la Ventana de Cambiar Indirectos y Utilidades para el Proyecto " & iprojectid & "', 'OK')")

    End Sub


    Private Sub AgregarProyectoCambiaIndirectosYUtilidad_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown

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


    Private Sub AgregarProyectoCambiaIndirectosYUtilidad_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Me.KeyPreview = True

        Me.AcceptButton = btnGuardar
        Me.CancelButton = btnCancelar

        closeTimedOutConnections()
        checkForKickoutsAndTimedOuts()
        setControlsByPermissions(Me.Name, susername)

        Dim queryIndirectos As String = ""
        Dim queryUtilidad As String = ""

        If isBase = True Then

            queryIndirectos = "SELECT dprojectindirectpercentagedefault * 100 FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & " WHERE ibaseid = " & iprojectid
            queryUtilidad = "SELECT dprojectgainpercentagedefault * 100 FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & " WHERE ibaseid = " & iprojectid

        Else

            If IsModel = True Then

                queryIndirectos = "SELECT dprojectindirectpercentagedefault * 100 FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & " WHERE imodelid = " & iprojectid
                queryUtilidad = "SELECT dprojectgainpercentagedefault * 100 FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & " WHERE imodelid = " & iprojectid

            Else

                queryIndirectos = "SELECT dprojectindirectpercentagedefault * 100 FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & " WHERE iprojectid = " & iprojectid
                queryUtilidad = "SELECT dprojectgainpercentagedefault * 100 FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & " WHERE iprojectid = " & iprojectid

            End If

        End If

        txtPorcentajeIndirectos.Text = getSQLQueryAsDouble(0, queryIndirectos)
        txtPorcentajeUtilidad.Text = getSQLQueryAsDouble(0, queryUtilidad)

        executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Abrió la Ventana de Cambiar Indirectos y Utilidades para el Proyecto " & iprojectid & "', 'OK')")

        txtPorcentajeIndirectos.Select()
        txtPorcentajeIndirectos.Focus()
        txtPorcentajeIndirectos.SelectionStart() = txtPorcentajeIndirectos.Text.Length

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


    Private Function validaCambiaIndirectos() As Boolean


        Dim strcaracteresprohibidos As String = "abcdefghijklmnopqrstuvwxyzñABCDEFGHIJKLMNOPQRSTUVWXYZÑ|°!#$%&/()=?¡*¨[]_:;,-{}+´¿'¬^`~@\<>"
        Dim arrayCaractProhib As Char() = strcaracteresprohibidos.ToCharArray
        txtPorcentajeIndirectos.Text = txtPorcentajeIndirectos.Text.Trim(arrayCaractProhib)
        txtPorcentajeUtilidad.Text = txtPorcentajeUtilidad.Text.Trim(arrayCaractProhib)



        If txtPorcentajeIndirectos.Text.Contains(".") Then

            Dim comparaPuntos As Char() = txtPorcentajeIndirectos.Text.ToCharArray
            Dim cuantosPuntos As Integer = 0


            For letra = 0 To comparaPuntos.Length - 1

                If comparaPuntos(letra) = "." Then
                    cuantosPuntos = cuantosPuntos + 1
                End If

            Next

            If cuantosPuntos > 1 Then

                For cantidad = 1 To cuantosPuntos
                    Dim lugar As Integer = txtPorcentajeIndirectos.Text.LastIndexOf(".")
                    Dim longitud As Integer = txtPorcentajeIndirectos.Text.Length

                    If longitud > (lugar + 1) Then
                        txtPorcentajeIndirectos.Text = txtPorcentajeIndirectos.Text.Substring(0, lugar) & txtPorcentajeIndirectos.Text.Substring(lugar + 1)
                        Exit For
                    Else
                        txtPorcentajeIndirectos.Text = txtPorcentajeIndirectos.Text.Substring(0, lugar)
                        Exit For
                    End If
                Next

            End If

        End If




        If txtPorcentajeUtilidad.Text.Contains(".") Then

            Dim comparaPuntos2 As Char() = txtPorcentajeUtilidad.Text.ToCharArray
            Dim cuantosPuntos2 As Integer = 0


            For letra = 0 To comparaPuntos2.Length - 1

                If comparaPuntos2(letra) = "." Then
                    cuantosPuntos2 = cuantosPuntos2 + 1
                End If

            Next

            If cuantosPuntos2 > 1 Then

                For cantidad = 1 To cuantosPuntos2
                    Dim lugar As Integer = txtPorcentajeUtilidad.Text.LastIndexOf(".")
                    Dim longitud As Integer = txtPorcentajeUtilidad.Text.Length

                    If longitud > (lugar + 1) Then
                        txtPorcentajeUtilidad.Text = txtPorcentajeUtilidad.Text.Substring(0, lugar) & txtPorcentajeUtilidad.Text.Substring(lugar + 1)
                        Exit For
                    Else
                        txtPorcentajeUtilidad.Text = txtPorcentajeUtilidad.Text.Substring(0, lugar)
                        Exit For
                    End If
                Next

            End If

        End If

        Dim indirectos As Double = 0.0
        Dim utilidad As Double = 0.0

        Try

            indirectos = CDbl(txtPorcentajeIndirectos.Text) / 100
            utilidad = CDbl(txtPorcentajeUtilidad.Text) / 100

        Catch ex As Exception

            MsgBox("¿Podrías verificar los porcentajes de Indirectos y Utilidad? Parecen no ser números válidos...", MsgBoxStyle.OkOnly, "Porcentajes No Válidos")
            Return False

        End Try

        Return True

    End Function


    Private Sub txtPorcentajeIndirectos_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtPorcentajeIndirectos.KeyUp

        Dim strcaracteresprohibidos As String = "abcdefghijklmnopqrstuvwxyzñABCDEFGHIJKLMNOPQRSTUVWXYZÑ|°!#$%&/()=?¡*¨[]_:;,-{}+´¿'¬^`~@\<>"
        Dim arrayCaractProhib As Char() = strcaracteresprohibidos.ToCharArray
        Dim resultado As Boolean = False

        For carp = 0 To arrayCaractProhib.Length - 1

            If txtPorcentajeIndirectos.Text.Contains(arrayCaractProhib(carp)) Then
                txtPorcentajeIndirectos.Text = txtPorcentajeIndirectos.Text.Replace(arrayCaractProhib(carp), "")
                resultado = True
            End If

        Next carp

        If txtPorcentajeIndirectos.Text.Contains(".") Then

            Dim comparaPuntos As Char() = txtPorcentajeIndirectos.Text.ToCharArray
            Dim cuantosPuntos As Integer = 0


            For letra = 0 To comparaPuntos.Length - 1

                If comparaPuntos(letra) = "." Then
                    cuantosPuntos = cuantosPuntos + 1
                End If

            Next

            If cuantosPuntos > 1 Then

                For cantidad = 1 To cuantosPuntos
                    Dim lugar As Integer = txtPorcentajeIndirectos.Text.LastIndexOf(".")
                    Dim longitud As Integer = txtPorcentajeIndirectos.Text.Length

                    If longitud > (lugar + 1) Then
                        txtPorcentajeIndirectos.Text = txtPorcentajeIndirectos.Text.Substring(0, lugar) & txtPorcentajeIndirectos.Text.Substring(lugar + 1)
                        resultado = True
                        Exit For
                    Else
                        txtPorcentajeIndirectos.Text = txtPorcentajeIndirectos.Text.Substring(0, lugar)
                        resultado = True
                        Exit For
                    End If
                Next

            End If

        End If

        If resultado = True Then
            txtPorcentajeIndirectos.Select(txtPorcentajeIndirectos.Text.Length, 0)
        End If

        txtPorcentajeIndirectos.Text = txtPorcentajeIndirectos.Text.Replace("--", "").Replace("'", "")
        txtPorcentajeIndirectos.Text = txtPorcentajeIndirectos.Text.Trim

    End Sub


    Private Sub txtPorcentajeUtilidad_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtPorcentajeUtilidad.KeyUp

        Dim strcaracteresprohibidos As String = "abcdefghijklmnopqrstuvwxyzñABCDEFGHIJKLMNOPQRSTUVWXYZÑ|°!#$%&/()=?¡*¨[]_:;,-{}+´¿'¬^`~@\<>"
        Dim arrayCaractProhib As Char() = strcaracteresprohibidos.ToCharArray
        Dim resultado As Boolean = False

        For carp = 0 To arrayCaractProhib.Length - 1

            If txtPorcentajeUtilidad.Text.Contains(arrayCaractProhib(carp)) Then
                txtPorcentajeUtilidad.Text = txtPorcentajeUtilidad.Text.Replace(arrayCaractProhib(carp), "")
                resultado = True
            End If

        Next carp

        If txtPorcentajeUtilidad.Text.Contains(".") Then

            Dim comparaPuntos As Char() = txtPorcentajeUtilidad.Text.ToCharArray
            Dim cuantosPuntos As Integer = 0


            For letra = 0 To comparaPuntos.Length - 1

                If comparaPuntos(letra) = "." Then
                    cuantosPuntos = cuantosPuntos + 1
                End If

            Next

            If cuantosPuntos > 1 Then

                For cantidad = 1 To cuantosPuntos
                    Dim lugar As Integer = txtPorcentajeUtilidad.Text.LastIndexOf(".")
                    Dim longitud As Integer = txtPorcentajeUtilidad.Text.Length

                    If longitud > (lugar + 1) Then
                        txtPorcentajeUtilidad.Text = txtPorcentajeUtilidad.Text.Substring(0, lugar) & txtPorcentajeUtilidad.Text.Substring(lugar + 1)
                        resultado = True
                        Exit For
                    Else
                        txtPorcentajeUtilidad.Text = txtPorcentajeUtilidad.Text.Substring(0, lugar)
                        resultado = True
                        Exit For
                    End If
                Next

            End If

        End If

        If resultado = True Then
            txtPorcentajeUtilidad.Select(txtPorcentajeUtilidad.Text.Length, 0)
        End If

        txtPorcentajeUtilidad.Text = txtPorcentajeUtilidad.Text.Replace("--", "").Replace("'", "")
        txtPorcentajeUtilidad.Text = txtPorcentajeUtilidad.Text.Trim

    End Sub


    Private Sub btnCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelar.Click

        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()

    End Sub


    Private Sub btnGuardar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGuardar.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        If validaCambiaIndirectos() = False Then

            Exit Sub

        End If

        Dim fecha As Integer = 0
        Dim hora As String = ""

        fecha = getMySQLDate()
        hora = getAppTime()

        Dim indirectos As Double = 0.0
        Dim utilidad As Double = 0.0

        Try

            indirectos = CDbl(txtPorcentajeIndirectos.Text) / 100
            utilidad = CDbl(txtPorcentajeUtilidad.Text) / 100

        Catch ex As Exception

        End Try

        If isBase = True Then

            executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards SET dcardindirectcostspercentage = " & indirectos & ", iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE ibaseid = " & iprojectid)
            executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Cards SET dcardgainpercentage = " & utilidad & ", iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE ibaseid = " & iprojectid)

        Else

            If IsModel = True Then

                executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Cards SET dcardindirectcostspercentage = " & indirectos & ", iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE imodelid = " & iprojectid)
                executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Cards SET dcardgainpercentage = " & utilidad & ", iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE imodelid = " & iprojectid)

            Else

                executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Cards SET dcardindirectcostspercentage = " & indirectos & ", iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE iprojectid = " & iprojectid)
                executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Cards SET dcardgainpercentage = " & utilidad & ", iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE iprojectid = " & iprojectid)

            End If

        End If

        executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó la Ventana de Cambiar Indirectos y Utilidades para el Proyecto " & iprojectid & "', 'OK')")

        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


End Class