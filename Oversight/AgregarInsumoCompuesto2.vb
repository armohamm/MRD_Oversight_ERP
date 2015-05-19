Imports Nevron.Chart.WinForm
Imports Nevron.Chart

Public Class AgregarInsumoCompuesto2

    Public susername As String = ""
    Public bactive As Boolean = False
    Public bonline As Boolean = False
    Public suserfirstname As String = ""
    Public suserlastname As String = ""
    Public suseremail As String = ""
    Public susersession As Integer = 0
    Public susermachinename As String = ""
    Public suserip As String = "0.0.0.0"

    Public permisoActivosCrear As Boolean = False
    Public permisoActivosEliminar As Boolean = False
    Public permisoActivosModificar As Boolean = False
    Public permisoActivosVer As Boolean = False
    Public permisoAnalisisDeUtilidadesCrear As Boolean = False
    Public permisoAnalisisDeUtilidadesEliminar As Boolean = False
    Public permisoAnalisisDeUtilidadesModificar As Boolean = False
    Public permisoAnalisisDeUtilidadesVer As Boolean = False
    Public permisoPersonasCrear As Boolean = False
    Public permisoPersonasEliminar As Boolean = False
    Public permisoPersonasModificar As Boolean = False
    Public permisoPersonasVer As Boolean = False
    Public permisoCotizacionesCrear As Boolean = False
    Public permisoCotizacionesEliminar As Boolean = False
    Public permisoCotizacionesModificar As Boolean = False
    Public permisoCotizacionesVer As Boolean = False
    Public permisoEnviosCrear As Boolean = False
    Public permisoEnviosEliminar As Boolean = False
    Public permisoEnviosModificar As Boolean = False
    Public permisoEnviosVer As Boolean = False
    Public permisoEquivalenciasCrear As Boolean = False
    Public permisoEquivalenciasEliminar As Boolean = False
    Public permisoEquivalenciasModificar As Boolean = False
    Public permisoEquivalenciasVer As Boolean = False
    Public permisoFacturasMRDCrear As Boolean = False
    Public permisoFacturasMRDEliminar As Boolean = False
    Public permisoFacturasMRDModificar As Boolean = False
    Public permisoFacturasMRDVer As Boolean = False
    Public permisoGastosAsignarAActivo As Boolean = False
    Public permisoGastosAsignarAProyecto As Boolean = False
    Public permisoGastosCrear As Boolean = False
    Public permisoGastosEliminar As Boolean = False
    Public permisoGastosModificar As Boolean = False
    Public permisoGastosVer As Boolean = False
    Public permisoIngresosCrear As Boolean = False
    Public permisoIngresosEliminar As Boolean = False
    Public permisoIngresosModificar As Boolean = False
    Public permisoIngresosVer As Boolean = False
    Public permisoInsumosCrear As Boolean = False
    Public permisoInsumosEliminar As Boolean = False
    Public permisoInsumosModificar As Boolean = False
    Public permisoInsumosVer As Boolean = False
    Public permisoInventarioCrear As Boolean = False
    Public permisoInventarioEliminar As Boolean = False
    Public permisoInventarioModificar As Boolean = False
    Public permisoInventarioVer As Boolean = False
    Public permisoLogsEliminar As Boolean = False
    Public permisoLogsVer As Boolean = False
    Public permisoModelosCrear As Boolean = False
    Public permisoModelosEliminar As Boolean = False
    Public permisoModelosModificar As Boolean = False
    Public permisoModelosPlanos As Boolean = False
    Public permisoModelosVer As Boolean = False
    Public permisoOrdenesCrear As Boolean = False
    Public permisoOrdenesEliminar As Boolean = False
    Public permisoOrdenesModificar As Boolean = False
    Public permisoOrdenesVer As Boolean = False
    Public permisoProveedoresCrear As Boolean = False
    Public permisoProveedoresEliminar As Boolean = False
    Public permisoProveedoresModificar As Boolean = False
    Public permisoProveedoresVer As Boolean = False
    Public permisoProyectosCrear As Boolean = False
    Public permisoProyectosEliminar As Boolean = False
    Public permisoProyectosModificar As Boolean = False
    Public permisoProyectosPlanos As Boolean = False
    Public permisoProyectosVer As Boolean = False
    Public permisoResumenDeTarjetasCrear As Boolean = False
    Public permisoResumenDeTarjetasEliminar As Boolean = False
    Public permisoResumenDeTarjetasModificar As Boolean = False
    Public permisoResumenDeTarjetasVer As Boolean = False
    Public permisoValesDeGasolinaCrear As Boolean = False
    Public permisoValesDeGasolinaEliminar As Boolean = False
    Public permisoValesDeGasolinaModificar As Boolean = False
    Public permisoValesDeGasolinaVer As Boolean = False

    Public iprojectid As Integer = 0
    Public icardid As Integer = 0
    Public iinputid As Integer = 0

    Public IsEdit As Boolean = False
    Public IsRecover As Boolean = False

    Public IsBase As Boolean = False
    Public IsModel As Boolean = False
    Public IsHistoric As Boolean = False

    Public iselectedinputid As Integer = 0
    Public sselectedinputdescription As String = ""
    Public sselectedunit As String = ""
    Public dselectedinputqty As Double = 0.0

    Public ihistoricprojectid As Integer = 0
    Public ihistoriccardid As Integer = 0

    Private isFormReadyForAction As Boolean = False
    Private isUnidadReady As Boolean = False

    Private WithEvents txtCantidadDgvInsumos As TextBox
    Private WithEvents txtNombreDgvInsumos As TextBox

    Private txtCantidadDgvInsumos_OldText As String = ""
    Private txtNombreDgvInsumos_OldText As String = ""



    Private Function FindControl(ByVal ControlName As String, ByVal CurrentControl As Control) As Control

        Dim ctr As Control

        For Each ctr In CurrentControl.Controls

            If ctr.Name = ControlName Then
                Return ctr
            Else

                ctr = FindControl(ControlName, ctr)
                If Not ctr Is Nothing Then
                    Return ctr
                End If

            End If

        Next ctr

    End Function


    Private Function FindToolStripMenuItem(ByVal ControlName As String) As ToolStripMenuItem

        Dim flp As FlowLayoutPanel
        Dim gb As GroupBox
        Dim sp As wyDay.Controls.SplitButton
        Dim tsmi As ToolStripMenuItem

        For Each flp In Me.Controls

            For Each gb In flp.Controls

                For Each sp In gb.Controls

                    For Each tsmi In sp.SplitMenuStrip.Items

                        If tsmi.Name = ControlName Then
                            Return tsmi
                        End If

                    Next tsmi

                Next sp

            Next gb

        Next flp

    End Function


    Private Function RightFor(ByVal username As String, ByVal windowname As String, ByVal controlname As String, ByVal attribToVerify As String) As Boolean

        Dim dsPermissions As DataSet
        dsPermissions = getSQLQueryAsDataset(0, "SELECT * FROM userpermissions2 WHERE susername = '" & username & "' AND swindowname = '" & windowname & "' AND scontrolname = '" & controlname & "'")

        If dsPermissions.Tables(0).Rows.Count > 0 Then

            For i = 0 To dsPermissions.Tables(0).Rows.Count - 1

                Dim attributesToChange(CStr(dsPermissions.Tables(0).Rows(i).Item("sattributestochangecsv")).Split(",").Length) As String
                attributesToChange = CStr(dsPermissions.Tables(0).Rows(i).Item("sattributestochangecsv")).Split(",")

                For x = 0 To attributesToChange.Length - 1

                    If attribToVerify = attributesToChange(x).Trim Then
                        Return True
                    End If

                Next x

                Return False

            Next i

        End If

    End Function


    Private Sub setControlsByPermissions(ByVal windowname As String, ByVal username As String)

        Dim dsPermissions As DataSet
        Dim controlname As String
        Dim objTypeFound As Boolean = False

        dsPermissions = getSQLQueryAsDataset(0, "SELECT * FROM userpermissions2 WHERE swindowname = '" & windowname & "' AND susername = '" & username & "'")

        If dsPermissions.Tables(0).Rows.Count = 0 Then
            Exit Sub
        End If

        For i = 0 To dsPermissions.Tables(0).Rows.Count - 1

            Try

                controlname = dsPermissions.Tables(0).Rows(i).Item("scontrolname")
                Dim attributesToChange(CStr(dsPermissions.Tables(0).Rows(i).Item("sattributestochangecsv")).Split(",").Length) As String
                attributesToChange = CStr(dsPermissions.Tables(0).Rows(i).Item("sattributestochangecsv")).Split(",")

                'TextBox

                If objTypeFound = False Then

                    Try

                        For x = 0 To attributesToChange.Length - 1

                            If attributesToChange(x).Trim = "Enabled" Then
                                DirectCast(FindControl(controlname, Me), TextBox).Enabled = True
                            End If

                            If attributesToChange(x).Trim = "Enabled" Then
                                DirectCast(FindControl(controlname, Me), TextBox).Visible = True
                            End If

                        Next x

                        objTypeFound = True

                    Catch ex As Exception

                    End Try

                End If


                'ComboBox

                If objTypeFound = False Then

                    Try

                        For x = 0 To attributesToChange.Length - 1

                            If attributesToChange(x).Trim = "Enabled" Then
                                DirectCast(FindControl(controlname, Me), ComboBox).Enabled = True
                            End If

                            If attributesToChange(x).Trim = "Enabled" Then
                                DirectCast(FindControl(controlname, Me), ComboBox).Visible = True
                            End If

                        Next x

                        objTypeFound = True

                    Catch ex As Exception

                    End Try

                End If


                'CheckBox

                If objTypeFound = False Then

                    Try

                        For x = 0 To attributesToChange.Length - 1

                            If attributesToChange(x).Trim = "Enabled" Then
                                DirectCast(FindControl(controlname, Me), CheckBox).Enabled = True
                            End If

                            If attributesToChange(x).Trim = "Enabled" Then
                                DirectCast(FindControl(controlname, Me), CheckBox).Visible = True
                            End If

                        Next x

                        objTypeFound = True

                    Catch ex As Exception

                    End Try

                End If


                'RadioButton

                If objTypeFound = False Then

                    Try

                        For x = 0 To attributesToChange.Length - 1

                            If attributesToChange(x).Trim = "Enabled" Then
                                DirectCast(FindControl(controlname, Me), RadioButton).Enabled = True
                            End If

                            If attributesToChange(x).Trim = "Enabled" Then
                                DirectCast(FindControl(controlname, Me), RadioButton).Visible = True
                            End If

                            If attributesToChange(x).Trim = "Checked" Then
                                DirectCast(FindControl(controlname, Me), RadioButton).Checked = True
                            End If

                        Next x

                        objTypeFound = True

                    Catch ex As Exception

                    End Try

                End If


                'DateTimePicker

                If objTypeFound = False Then

                    Try

                        For x = 0 To attributesToChange.Length - 1

                            If attributesToChange(x).Trim = "Enabled" Then
                                DirectCast(FindControl(controlname, Me), DateTimePicker).Enabled = True
                            End If

                            If attributesToChange(x).Trim = "Enabled" Then
                                DirectCast(FindControl(controlname, Me), DateTimePicker).Visible = True
                            End If

                        Next x

                        objTypeFound = True

                    Catch ex As Exception

                    End Try

                End If


                'wyDay.Controls.SplitButton

                If objTypeFound = False Then

                    Try

                        For x = 0 To attributesToChange.Length - 1

                            If attributesToChange(x).Trim = "Enabled" Then
                                DirectCast(FindControl(controlname, Me), wyDay.Controls.SplitButton).Enabled = True
                            End If

                            If attributesToChange(x).Trim = "Enabled" Then
                                DirectCast(FindControl(controlname, Me), wyDay.Controls.SplitButton).Visible = True
                            End If

                        Next x

                        objTypeFound = True

                    Catch ex As Exception

                    End Try

                End If


                'Button

                If objTypeFound = False Then

                    Try

                        For x = 0 To attributesToChange.Length - 1

                            If attributesToChange(x).Trim = "Enabled" Then
                                DirectCast(FindControl(controlname, Me), Button).Enabled = True
                            End If

                            If attributesToChange(x).Trim = "Visible" Then
                                DirectCast(FindControl(controlname, Me), Button).Visible = True
                            End If

                        Next x

                        objTypeFound = True

                    Catch ex As Exception

                    End Try

                End If


                'ToolStripMenuItem

                If objTypeFound = False Then

                    Try

                        For x = 0 To attributesToChange.Length - 1

                            If attributesToChange(x).Trim = "Enabled" Then
                                DirectCast(FindToolStripMenuItem(controlname), ToolStripMenuItem).Enabled = True
                            End If

                            If attributesToChange(x).Trim = "Visible" Then
                                DirectCast(FindToolStripMenuItem(controlname), ToolStripMenuItem).Visible = True
                            End If

                        Next x

                        objTypeFound = True

                    Catch ex As Exception

                    End Try

                End If


                'DataGridView

                If objTypeFound = False Then

                    Try

                        For x = 0 To attributesToChange.Length - 1

                            If attributesToChange(x).Trim = "Enabled" Then
                                DirectCast(FindControl(controlname, Me), DataGridView).Enabled = True
                            End If

                            If attributesToChange(x).Trim = "Visible" Then
                                DirectCast(FindControl(controlname, Me), DataGridView).Visible = True
                            End If

                            If attributesToChange(x).Trim = "ReadOnly" Then
                                DirectCast(FindControl(controlname, Me), DataGridView).ReadOnly = True
                            End If

                        Next x

                        objTypeFound = True

                    Catch ex As Exception

                    End Try

                End If


                'TabPage

                If objTypeFound = False Then

                    Try

                        For x = 0 To attributesToChange.Length - 1

                            If attributesToChange(x).Trim = "Enabled" Then
                                DirectCast(FindControl(controlname, Me), TabPage).Enabled = True
                            End If

                            If attributesToChange(x).Trim = "Visible" Then
                                DirectCast(FindControl(controlname, Me), TabPage).Visible = True
                            End If

                        Next x

                        objTypeFound = True

                    Catch ex As Exception

                    End Try

                End If


            Catch ex As Exception

            End Try

            controlname = ""

        Next i

    End Sub


    Private Sub checkMessages(ByVal username As String, ByVal x As Integer, ByVal y As Integer)

        Dim unreadmessagecount As Integer = 0
        unreadmessagecount = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM messages where susername = '" & username & "' AND bread = 0")

        If unreadmessagecount > 0 Then

            Dim msg As New Mensajes
            Dim pt As Point

            msg.susername = susername
            msg.bactive = bactive
            msg.bonline = bonline
            msg.suserfirstname = suserfirstname
            msg.suserlastname = suserlastname
            msg.suseremail = suseremail
            msg.susersession = susersession
            msg.susermachinename = susermachinename
            msg.suserip = suserip

            msg.StartPosition = FormStartPosition.Manual

            pt = New Point(x, y - 120)

            If pt.X < Screen.GetWorkingArea(Me).Location.X Or pt.Y < Screen.GetWorkingArea(Me).Location.Y Then

                pt = New Point(x, y)

            End If

            msg.Location = pt

            msg.Show()

        End If

    End Sub


    Private Sub verifySuspiciousData()


        If getSQLQueryAsInteger(0, "SELECT COUNT(pcci.iprojectid) AS conteo FROM projectcardcompoundinputs pcci JOIN projects p ON pcci.iprojectid = p.iprojectid WHERE p.iprojectstarteddate > 0 AND STR_TO_DATE(CONCAT(pcci.iupdatedate, ' ', pcci.supdatetime), '%Y%c%d %T')  > STR_TO_DATE(CONCAT(p.iprojectstarteddate, ' ', p.sprojectstartedtime), '%Y%c%d %T')") > 0 Then

            Dim fecha As Integer = getAppDate()
            Dim hora As String = getAppTime()

            executeSQLCommand(0, "INSERT INTO logs VALUES (" & fecha & ", '" & hora & "', 'SYSTEM', 0, '127.0.0.1', 'Myself', 'Movimiento fuera de tiempo; probable intrusión o encubrimiento (ProjectCardCompoundInputs)', 'Detected/Reported')")
            executeSQLCommand(0, "INSERT INTO messages (susername, susersession, smessage, bread, imessagedate, smessagetime, iupdatedate, supdatetime, supdateusername) VALUES ('SYSTEM', 0, 'Ha sido detectado un movimiento sospechoso (ProjectCardCompoundInputs). ¿Podrías revisarlo?', 0, " & fecha & ", '" & hora & "', " & fecha & ", '" & hora & "', 'SYSTEM')")

        End If

        If getSQLQueryAsInteger(0, "SELECT COUNT(pci.iprojectid) AS conteo FROM projectcardinputs pci JOIN projects p ON pci.iprojectid = p.iprojectid WHERE p.iprojectstarteddate > 0 AND STR_TO_DATE(CONCAT(pci.iupdatedate, ' ', pci.supdatetime), '%Y%c%d %T')  > STR_TO_DATE(CONCAT(p.iprojectstarteddate, ' ', p.sprojectstartedtime), '%Y%c%d %T')") > 0 Then

            Dim fecha As Integer = getAppDate()
            Dim hora As String = getAppTime()

            executeSQLCommand(0, "INSERT INTO logs VALUES (" & fecha & ", '" & hora & "', 'SYSTEM', 0, '127.0.0.1', 'Myself', 'Movimiento fuera de tiempo; probable intrusión o encubrimiento (ProjectCardInputs)', 'Detected/Reported')")
            executeSQLCommand(0, "INSERT INTO messages (susername, susersession, smessage, bread, imessagedate, smessagetime, iupdatedate, supdatetime, supdateusername) VALUES ('SYSTEM', 0, 'Ha sido detectado un movimiento sospechoso (ProjectCardInputs). ¿Podrías revisarlo?', 0, " & fecha & ", '" & hora & "', " & fecha & ", '" & hora & "', 'SYSTEM')")

        End If

        If getSQLQueryAsInteger(0, "SELECT COUNT(pc.iprojectid) AS conteo FROM projectcards pc JOIN projects p ON pc.iprojectid = p.iprojectid WHERE p.iprojectstarteddate > 0 AND STR_TO_DATE(CONCAT(pc.iupdatedate, ' ', pc.supdatetime), '%Y%c%d %T')  > STR_TO_DATE(CONCAT(p.iprojectstarteddate, ' ', p.sprojectstartedtime), '%Y%c%d %T')") > 0 Then

            Dim fecha As Integer = getAppDate()
            Dim hora As String = getAppTime()

            executeSQLCommand(0, "INSERT INTO logs VALUES (" & fecha & ", '" & hora & "', 'SYSTEM', 0, '127.0.0.1', 'Myself', 'Movimiento fuera de tiempo; probable intrusión o encubrimiento (ProjectCards)', 'Detected/Reported')")
            executeSQLCommand(0, "INSERT INTO messages (susername, susersession, smessage, bread, imessagedate, smessagetime, iupdatedate, supdatetime, supdateusername) VALUES ('SYSTEM', 0, 'Ha sido detectado un movimiento sospechoso (ProjectCards). ¿Podrías revisarlo?', 0, " & fecha & ", '" & hora & "', " & fecha & ", '" & hora & "', 'SYSTEM')")

        End If

        If getSQLQueryAsInteger(0, "SELECT COUNT(pic.iprojectid) AS conteo FROM projectindirectcosts pic JOIN projects p ON pic.iprojectid = p.iprojectid WHERE p.iprojectstarteddate > 0 AND STR_TO_DATE(CONCAT(pic.iupdatedate, ' ', pic.supdatetime), '%Y%c%d %T')  > STR_TO_DATE(CONCAT(p.iprojectstarteddate, ' ', p.sprojectstartedtime), '%Y%c%d %T')") > 0 Then

            Dim fecha As Integer = getAppDate()
            Dim hora As String = getAppTime()

            executeSQLCommand(0, "INSERT INTO logs VALUES (" & fecha & ", '" & hora & "', 'SYSTEM', 0, '127.0.0.1', 'Myself', 'Movimiento fuera de tiempo; probable intrusión o encubrimiento (ProjectIndirectCosts)', 'Detected/Reported')")
            executeSQLCommand(0, "INSERT INTO messages (susername, susersession, smessage, bread, imessagedate, smessagetime, iupdatedate, supdatetime, supdateusername) VALUES ('SYSTEM', 0, 'Ha sido detectado un movimiento sospechoso (ProjectIndirectCosts). ¿Podrías revisarlo?', 0, " & fecha & ", '" & hora & "', " & fecha & ", '" & hora & "', 'SYSTEM')")

        End If

        If getSQLQueryAsInteger(0, "SELECT COUNT(sip.iprojectid) AS conteo FROM supplierinvoiceprojects sip JOIN projects p ON sip.iprojectid = p.iprojectid WHERE p.iprojectstarteddate > 0 AND STR_TO_DATE(CONCAT(sip.iupdatedate, ' ', sip.supdatetime), '%Y%c%d %T')  < STR_TO_DATE(CONCAT(p.iprojectstarteddate, ' ', p.sprojectstartedtime), '%Y%c%d %T')") > 0 Then

            Dim fecha As Integer = getAppDate()
            Dim hora As String = getAppTime()

            executeSQLCommand(0, "INSERT INTO logs VALUES (" & fecha & ", '" & hora & "', 'SYSTEM', 0, '127.0.0.1', 'Myself', 'Movimiento fuera de tiempo; probable intrusión o encubrimiento (SupplierInvoiceProjects))', 'Detected/Reported')")
            executeSQLCommand(0, "INSERT INTO messages (susername, susersession, smessage, bread, imessagedate, smessagetime, iupdatedate, supdatetime, supdateusername) VALUES ('SYSTEM', 0, 'Ha sido detectado un movimiento sospechoso (SupplierInvoiceProjects). ¿Podrías revisarlo?', 0, " & fecha & ", '" & hora & "', " & fecha & ", '" & hora & "', 'SYSTEM')")

        End If

        If getSQLQueryAsInteger(0, "SELECT COUNT(*) AS conteo FROM supplierinvoiceinputs WHERE dsupplierinvoiceinputtotalprice <= 0 OR dsupplierinvoiceinputunitprice <= 0") > 0 Then

            Dim fecha As Integer = getAppDate()
            Dim hora As String = getAppTime()

            executeSQLCommand(0, "INSERT INTO logs VALUES (" & fecha & ", '" & hora & "', 'SYSTEM', 0, '127.0.0.1', 'Myself', 'Movimiento fuera de tiempo; probable intrusión o encubrimiento (SupplierInvoiceInputs)', 'Detected/Reported')")
            executeSQLCommand(0, "INSERT INTO messages (susername, susersession, smessage, bread, imessagedate, smessagetime, iupdatedate, supdatetime, supdateusername) VALUES ('SYSTEM', 0, 'Ha sido detectado un movimiento sospechoso (SupplierInvoiceInputs). ¿Podrías revisarlo?', 0, " & fecha & ", '" & hora & "', " & fecha & ", '" & hora & "', 'SYSTEM')")

        End If

        If getSQLQueryAsInteger(0, "SELECT COUNT(*) AS conteo FROM payments WHERE dpaymentamount <= 0") > 0 Then

            Dim fecha As Integer = getAppDate()
            Dim hora As String = getAppTime()

            executeSQLCommand(0, "INSERT INTO logs VALUES (" & fecha & ", '" & hora & "', 'SYSTEM', 0, '127.0.0.1', 'Myself', 'Movimiento fuera de tiempo; probable intrusión o encubrimiento (Payments)', 'Detected/Reported')")
            executeSQLCommand(0, "INSERT INTO messages (susername, susersession, smessage, bread, imessagedate, smessagetime, iupdatedate, supdatetime, supdateusername) VALUES ('SYSTEM', 0, 'Ha sido detectado un movimiento sospechoso (Payments). ¿Podrías revisarlo?', 0, " & fecha & ", '" & hora & "', " & fecha & ", '" & hora & "', 'SYSTEM')")

        End If

        If getSQLQueryAsInteger(0, "SELECT COUNT(*) AS conteo FROM projects p WHERE p.iprojectstarteddate > 0 AND STR_TO_DATE(CONCAT(p.iupdatedate, ' ', p.supdatetime), '%Y%c%d %T')  > STR_TO_DATE(CONCAT(p.iprojectstarteddate, ' ', p.sprojectstartedtime), '%Y%c%d %T')") > 0 Then

            Dim fecha As Integer = getAppDate()
            Dim hora As String = getAppTime()

            executeSQLCommand(0, "INSERT INTO logs VALUES (" & fecha & ", '" & hora & "', 'SYSTEM', 0, '127.0.0.1', 'Myself', 'Movimiento fuera de tiempo; probable intrusión o encubrimiento (Projects)', 'Detected/Reported')")
            executeSQLCommand(0, "INSERT INTO messages (susername, susersession, smessage, bread, imessagedate, smessagetime, iupdatedate, supdatetime, supdateusername) VALUES ('SYSTEM', 0, 'Ha sido detectado un movimiento sospechoso (Projects). ¿Podrías revisarlo?', 0, " & fecha & ", '" & hora & "', " & fecha & ", '" & hora & "', 'SYSTEM')")

        End If

    End Sub


    Private Sub AgregarInsumoCompuesto_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim conteo1 As Integer = 0
        Dim conteo2 As Integer = 0
        Dim conteo3 As Integer = 0
        Dim conteo4 As Integer = 0
        Dim conteo5 As Integer = 0
        Dim conteo6 As Integer = 0
        Dim conteo7 As Integer = 0
        Dim conteo8 As Integer = 0
        Dim conteo9 As Integer = 0

        Dim unsaved As Boolean = False

        conteo1 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(*) " & _
        "FROM inputs " & _
        "WHERE iinputid = " & iinputid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " ti WHERE inputs.iinputid = ti.iinputid) ")

        conteo2 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(*) " & _
        "FROM inputtypes " & _
        "WHERE iinputid = " & iinputid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types tit WHERE inputtypes.iinputid = tit.iinputid) ")

        conteo3 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(*) " & _
        "FROM compoundinputs " & _
        "WHERE iinputid = " & iinputid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompoundInput" & iinputid & " tci WHERE compoundinputs.iinputid = tci.iinputid AND compoundinputs.icompoundinputid = tci.icompoundinputid) ")

        conteo4 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(tit.*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types tit JOIN inputtypes it ON tit.iinputid = it.iinputid WHERE STR_TO_DATE(CONCAT(tit.iupdatedate, ' ', tit.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(it.iupdatedate, ' ', it.supdatetime), '%Y%c%d %T') ")

        conteo5 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(ti.*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " ti JOIN inputs i ON ti.iinputid = i.iinputid WHERE STR_TO_DATE(CONCAT(ti.iupdatedate, ' ', ti.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(i.iupdatedate, ' ', i.supdatetime), '%Y%c%d %T') ")

        conteo6 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(tci.*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompoundInput" & iinputid & " tci JOIN compoundinputs ci ON tci.iinputid = ci.iinputid WHERE STR_TO_DATE(CONCAT(tci.iupdatedate, ' ', tci.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(ci.iupdatedate, ' ', ci.supdatetime), '%Y%c%d %T') ")

        conteo7 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(*) " & _
        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " ti " & _
        "WHERE NOT EXISTS (SELECT * FROM inputs i WHERE i.iinputid = ti.iinputid AND i.iinputid = " & iinputid & ") ")

        conteo8 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(*) " & _
        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types tit " & _
        "WHERE NOT EXISTS (SELECT * FROM inputtypes it WHERE it.iinputid = tit.iinputid AND it.iinputid = " & iinputid & ") ")

        conteo9 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(*) " & _
        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompoundInput" & iinputid & " tci " & _
        "WHERE NOT EXISTS (SELECT * FROM compoundinputs ci WHERE ci.iinputid = tci.iinputid AND ci.icompoundinputid = tci.icompoundinputid AND ci.iinputid = " & iinputid & ") ")

        If conteo1 + conteo2 + conteo3 + conteo4 + conteo5 + conteo6 + conteo7 + conteo8 + conteo9 > 0 Then

            unsaved = True

        End If

        Dim incomplete As Boolean = False
        Dim msg As String = ""
        Dim result As Integer = 0

        If validaInsumosCompuestos(True) = False And Me.DialogResult <> Windows.Forms.DialogResult.OK And IsHistoric = False Then
            incomplete = True
        End If

        If incomplete = True Then
            result = MsgBox("Este Insumo no está completo. Si sales ahora, se perderán los cambios que hayas hecho a este Insumo y por consiguiente, a las Tarjetas que lo contengan" & Chr(13) & "¿Realmente deseas Salir de esta ventana ahora?", MsgBoxStyle.YesNo, "Confirmación Salida")
        ElseIf unsaved = True Then
            result = MsgBox("Tienes datos sin guardar! Tienes 3 opciones: " & Chr(13) & "Guardar los cambios (Sí), Regresar a revisar los cambios y guardarlos manualmente (Cancelar) o No guardarlos (No)", MsgBoxStyle.YesNoCancel, "Confirmación Salida")
        End If

        If result = MsgBoxResult.No And incomplete = True Then

            Cursor.Current = System.Windows.Forms.Cursors.Default
            Exit Sub

        ElseIf result = MsgBoxResult.Yes And incomplete = False Then


            Dim timesInputIsOpen As Integer = 1

            timesInputIsOpen = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%Input" & iinputid & "'")

            If timesInputIsOpen > 1 Then

                Cursor.Current = System.Windows.Forms.Cursors.Default

                If MsgBox("Otra persona tiene abierto el mismo Insumo. Esto podría causar que alguno de ustedes perdiera los cambios que hiciera. ¿Deseas seguir abriendo el Proyecto?", MsgBoxStyle.YesNo, "Confirmación Apertura") = MsgBoxResult.No Then

                    Me.DialogResult = Windows.Forms.DialogResult.Cancel
                    Me.Close()

                    Cursor.Current = System.Windows.Forms.Cursors.Default
                    Exit Sub

                Else

                    Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

                End If

            End If

            Dim queriesSave(10) As String

            queriesSave(0) = "" & _
            "DELETE " & _
            "FROM inputs " & _
            "WHERE iinputid = " & iinputid & " AND " & _
            "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " ti WHERE inputs.iinputid = ti.iinputid) "

            queriesSave(1) = "" & _
            "DELETE " & _
            "FROM inputtypes " & _
            "WHERE iinputid = " & iinputid & " AND " & _
            "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types tit WHERE inputtypes.iinputid = tit.iinputid) "

            queriesSave(2) = "" & _
            "DELETE " & _
            "FROM compoundinputs " & _
            "WHERE iinputid = " & iinputid & " AND " & _
            "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompoundInput" & iinputid & " tci WHERE compoundinputs.iinputid = tci.iinputid AND compoundinputs.icompoundinputid = tci.icompoundinputid) "

            queriesSave(3) = "" & _
            "UPDATE inputs i JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " ti ON ti.iinputid = i.iinputid SET i.iupdatedate = ti.iupdatedate, i.supdatetime = ti.supdatetime, i.supdateusername = ti.supdateusername, i.sinputdescription = ti.sinputdescription, i.sinputunit = ti.sinputunit WHERE STR_TO_DATE(CONCAT(ti.iupdatedate, ' ', ti.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(i.iupdatedate, ' ', i.supdatetime), '%Y%c%d %T') "

            queriesSave(4) = "" & _
            "UPDATE inputtypes it JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types tit ON tit.iinputid = it.iinputid SET it.iupdatedate = tit.iupdatedate, it.supdatetime = tit.supdatetime, it.supdateusername = tit.supdateusername, it.sinputtypedescription = tit.sinputtypedescription WHERE STR_TO_DATE(CONCAT(tit.iupdatedate, ' ', tit.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(it.iupdatedate, ' ', it.supdatetime), '%Y%c%d %T') "

            queriesSave(5) = "" & _
            "UPDATE compoundinputs ci JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompoundInput" & iinputid & " tci ON tci.iinputid = ci.iinputid SET ci.iupdatedate = tci.iupdatedate, ci.supdatetime = tci.supdatetime, ci.supdateusername = tci.supdateusername, ci.icompoundinputid = tci.icompoundinputid WHERE STR_TO_DATE(CONCAT(tci.iupdatedate, ' ', tci.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(ci.iupdatedate, ' ', ci.supdatetime), '%Y%c%d %T') "

            queriesSave(6) = "" & _
            "INSERT INTO inputs " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " ti " & _
            "WHERE NOT EXISTS (SELECT * FROM inputs i WHERE i.iinputid = ti.iinputid AND i.iinputid = " & iinputid & ") "

            queriesSave(7) = "" & _
            "INSERT INTO inputtypes " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types tit " & _
            "WHERE NOT EXISTS (SELECT * FROM inputtypes it WHERE it.iinputid = tit.iinputid AND it.iinputid = " & iinputid & ") "

            queriesSave(8) = "" & _
            "INSERT INTO compoundinputs " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompoundInput" & iinputid & " tci " & _
            "WHERE NOT EXISTS (SELECT * FROM compoundinputs ci WHERE ci.iinputid = tci.iinputid AND ci.icompoundinputid = tci.icompoundinputid AND ci.iinputid = " & iinputid & ") "

            queriesSave(9) = "INSERT INTO logs VALUES (" & getAppDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " : " & txtNombreDelInsumo.Text & "', 'OK')"

            executeTransactedSQLCommand(0, queriesSave)


        ElseIf result = MsgBoxResult.Cancel Then

            Cursor.Current = System.Windows.Forms.Cursors.Default
            Exit Sub

        End If


        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim queriesDelete(4) As String

        queriesDelete(0) = "DROP TABLE IF EXISTS oversight. tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid
        queriesDelete(1) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompoundInput" & iinputid
        queriesDelete(2) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types"
        queriesDelete(3) = "INSERT INTO logs VALUES (" & getAppDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Cerró el Insumo Compuesto " & iinputid & ": " & txtNombreDelInsumo.Text & "', 'OK')"

        executeTransactedSQLCommand(0, queriesDelete)

        verifySuspiciousData()

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub AgregarInsumoCompuesto_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Me.AcceptButton = btnGuardar
        Me.CancelButton = btnCancelar

        setControlsByPermissions(Me.Name, susername)
        checkMessages(susername, Me.Location.X, Me.Location.Y)

        Dim timesInputIsOpen As Integer = 1

        timesInputIsOpen = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%Input" & iinputid & "'")

        If timesInputIsOpen > 1 Then

            Cursor.Current = System.Windows.Forms.Cursors.Default

            If MsgBox("Otra persona tiene abierto el mismo Insumo. Esto podría causar que alguno de ustedes perdiera los cambios que hiciera. ¿Deseas seguir abriendo el Proyecto?", MsgBoxStyle.YesNo, "Confirmación Apertura") = MsgBoxResult.No Then

                Me.DialogResult = Windows.Forms.DialogResult.Cancel
                Me.Close()

                Cursor.Current = System.Windows.Forms.Cursors.Default
                Exit Sub

            Else

                Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

            End If

        End If


        If IsRecover = False Then

            Dim queriesCreation(6) As String

            queriesCreation(0) = "DROP TABLE IF EXISTS oversight. tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid
            queriesCreation(1) = "CREATE TABLE  oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " ( `iinputid` int(11) NOT NULL AUTO_INCREMENT, `sinputdescription` varchar(300) CHARACTER SET latin1 NOT NULL, `sinputunit` varchar(100) CHARACTER SET latin1 NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL,  PRIMARY KEY (`iinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesCreation(2) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompoundInput" & iinputid
            queriesCreation(3) = "CREATE TABLE  oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompoundInput" & iinputid & " ( `iinputid` int(11) NOT NULL, `icompoundinputid` int(11) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`iinputid`,`icompoundinputid`) USING BTREE, KEY `inputid` (`iinputid`), KEY `compoundinputid` (`icompoundinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesCreation(4) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types"
            queriesCreation(5) = "CREATE TABLE  oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types" & " ( `iinputid` int(11) NOT NULL, `sinputtypedescription` varchar(250) CHARACTER SET latin1 NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`iinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci COMMENT='Only to differ which input is taken as Mano de Obra'"

            executeTransactedSQLCommand(0, queriesCreation)

        End If


        If IsEdit = True Then

            Dim queryInsumo As String
            Dim dsInsumo As DataSet

            If IsRecover = False Then

                Dim queriesInsert(3) As String

                queriesInsert(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " SELECT * FROM inputs WHERE iinputid = " & iinputid
                queriesInsert(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types SELECT * FROM inputtypes WHERE iinputid = " & iinputid
                queriesInsert(2) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompoundInput" & iinputid & " SELECT * FROM compoundinputs WHERE iinputid = " & iinputid

                executeTransactedSQLCommand(0, queriesInsert)

            End If

            queryInsumo = "SELECT i.iinputid, i.sinputdescription, i.sinputunit, it.sinputtypedescription " & _
            "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " i " & _
            "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types it ON it.iinputid = i.iinputid " & _
            "WHERE i.iinputid = " & iinputid

            dsInsumo = getSQLQueryAsDataset(0, queryInsumo)

            txtNombreDelInsumo.Text = dsInsumo.Tables(0).Rows(0).Item("sinputdescription")
            cmbTipoDeInsumo.SelectedItem = dsInsumo.Tables(0).Rows(0).Item("sinputtypedescription")
            txtUnidadDeMedida.Text = dsInsumo.Tables(0).Rows(0).Item("sinputunit")


            Dim querySumaInsumoCompuesto As String = ""
            Dim sumaInsumoCompuesto As Double

            If IsBase = True Then

                querySumaInsumoCompuesto = "" & _
                "SELECT SUM(btfci.dcompoundinputqty*bp.dinputfinalprice) AS precio " & _
                "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs btfci " & _
                "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompoundInput" & iinputid & " ci ON ci.icompoundinputid = btfci.icompoundinputid " & _
                "LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " i ON i.iinputid = btfci.icompoundinputid " & _
                "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfci.ibaseid = bp.ibaseid AND btfci.icompoundinputid = bp.iinputid " & _
                "WHERE btfci.ibaseid = " & iprojectid & " and btfci.icardid = " & icardid & " " & _
                "GROUP BY ci.iinputid "

            Else

                If IsModel = True Then

                    querySumaInsumoCompuesto = "" & _
                    "SELECT SUM(mtfci.dcompoundinputqty*mp.dinputfinalprice) AS precio " & _
                    "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs mtfci " & _
                    "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompoundInput" & iinputid & " ci ON ci.icompoundinputid = mtfci.icompoundinputid " & _
                    "LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " i ON i.iinputid = mtfci.icompoundinputid " & _
                    "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) mp GROUP BY iinputid, imodelid) mp ON mtfci.imodelid = mp.imodelid AND mtfci.icompoundinputid = mp.iinputid " & _
                    "WHERE mtfci.imodelid = " & iprojectid & " and mtfci.icardid = " & icardid & " " & _
                    "GROUP BY ci.iinputid "

                Else

                    querySumaInsumoCompuesto = "" & _
                    "SELECT SUM(ptfci.dcompoundinputqty*pp.dinputfinalprice) AS precio " & _
                    "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs ptfci " & _
                    "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompoundInput" & iinputid & " ci ON ci.icompoundinputid = ptfci.icompoundinputid " & _
                    "LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " i ON i.iinputid = ptfci.icompoundinputid " & _
                    "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, iprojectid) pp ON ptfci.iprojectid = pp.iprojectid AND ptfci.icompoundinputid = pp.iinputid " & _
                    "WHERE ptfci.iprojectid = " & iprojectid & " and ptfci.icardid = " & icardid & " " & _
                    "GROUP BY ci.iinputid "

                End If

            End If


            sumaInsumoCompuesto = getSQLQueryAsDouble(0, querySumaInsumoCompuesto)

            txtCostoParaTabulador.Text = FormatCurrency(sumaInsumoCompuesto, 2, TriState.True, TriState.False, TriState.True).Replace("$", "")


            If IsHistoric = True Then

                txtNombreDelInsumo.Enabled = False
                cmbTipoDeInsumo.Enabled = False
                txtUnidadDeMedida.Enabled = False
                dgvInsumos.ReadOnly = True
                txtCostoParaTabulador.Enabled = False

            Else

                dgvInsumos.Enabled = True
                dgvInsumos.ReadOnly = False

            End If


        End If


        Dim queryInsumos As String = ""

        If IsBase = True Then

            queryInsumos = "" & _
            "SELECT btfci.icompoundinputid, i.sinputdescription AS 'Insumo', btfci.scompoundinputunit AS 'Unidad', btfci.dcompoundinputqty AS 'Cantidad', " & _
            "FORMAT((btfci.dcompoundinputqty*bp.dinputfinalprice), 3) AS 'Precio' " & _
            "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs btfci " & _
            "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompoundInput" & iinputid & " ci ON ci.icompoundinputid = btfci.icompoundinputid " & _
            "LEFT JOIN inputs i ON i.iinputid = btfci.icompoundinputid " & _
            "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfci.ibaseid = bp.ibaseid AND btfci.icompoundinputid = bp.iinputid " & _
            "WHERE btfci.ibaseid = " & iprojectid & " AND btfci.icardid = " & icardid & " AND ci.iinputid = " & iinputid

        Else

            If IsModel = True Then

                queryInsumos = "" & _
                "SELECT mtfci.icompoundinputid, i.sinputdescription AS 'Insumo', mtfci.scompoundinputunit AS 'Unidad', mtfci.dcompoundinputqty AS 'Cantidad', " & _
                "FORMAT((mtfci.dcompoundinputqty*mp.dinputfinalprice), 3) AS 'Precio' " & _
                "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs mtfci " & _
                "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompoundInput" & iinputid & " ci ON ci.icompoundinputid = mtfci.icompoundinputid " & _
                "LEFT JOIN inputs i ON i.iinputid = mtfci.icompoundinputid " & _
                "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) mp GROUP BY iinputid, imodelid) mp ON mtfci.imodelid = mp.imodelid AND mtfci.icompoundinputid = mp.iinputid " & _
                "WHERE mtfci.imodelid = " & iprojectid & " AND mtfci.icardid = " & icardid & " AND ci.iinputid = " & iinputid

            Else

                queryInsumos = "" & _
                "SELECT ptfci.icompoundinputid, i.sinputdescription AS 'Insumo', ptfci.scompoundinputunit AS 'Unidad', FORMAT(ptfci.dcompoundinputqty, 3) AS 'Cantidad', " & _
                "FORMAT((ptfci.dcompoundinputqty*pp.dinputfinalprice), 3) AS 'Precio' " & _
                "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs ptfci " & _
                "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompoundInput" & iinputid & " ci ON ci.icompoundinputid = ptfci.icompoundinputid " & _
                "LEFT JOIN inputs i ON i.iinputid = ptfci.icompoundinputid " & _
                "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, iprojectid) pp ON ptfci.iprojectid = pp.iprojectid AND ptfci.icompoundinputid = pp.iinputid " & _
                "WHERE ptfci.iprojectid = " & iprojectid & " AND ptfci.icardid = " & icardid & " AND ci.iinputid = " & iinputid

            End If

        End If


        setDataGridView(dgvInsumos, queryInsumos, False)

        dgvInsumos.Columns(0).Visible = False

        dgvInsumos.Columns(0).ReadOnly = True
        dgvInsumos.Columns(2).ReadOnly = True
        dgvInsumos.Columns(4).ReadOnly = True


        Dim queryPreciosHistoricosInsumoCompuesto As String = ""

        If IsBase = True Then

            queryPreciosHistoricosInsumoCompuesto = "" & _
            "SELECT btfci.ibaseid, btfci.icardid, STR_TO_DATE(CONCAT(btfci.iupdatedate, ' ', btfci.supdatetime), '%Y%c%d %T') AS 'Fecha Hora', " & _
            "FORMAT(SUM(btfci.dcompoundinputqty*bp.dinputfinalprice), 3) AS 'Precio' " & _
            "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs btfci " & _
            "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompoundInput" & iinputid & " ci ON ci.icompoundinputid = btfci.icompoundinputid " & _
            "LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " i ON i.iinputid = btfci.icompoundinputid " & _
            "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfci.ibaseid = bp.ibaseid AND btfci.icompoundinputid = bp.iinputid " & _
            "GROUP BY btfci.ibaseid, btfci.icardid, ci.iinputid "

        Else

            If IsModel = True Then

                queryPreciosHistoricosInsumoCompuesto = "" & _
                "SELECT mtfci.imodelid, mtfci.icardid, STR_TO_DATE(CONCAT(mtfci.iupdatedate, ' ', mtfci.supdatetime), '%Y%c%d %T') AS 'Fecha Hora', " & _
                "FORMAT(SUM(mtfci.dcompoundinputqty*mp.dinputfinalprice), 3) AS 'Precio' " & _
                "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs mtfci " & _
                "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompoundInput" & iinputid & " ci ON ci.icompoundinputid = mtfci.icompoundinputid " & _
                "LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " i ON i.iinputid = mtfci.icompoundinputid " & _
                "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) mp GROUP BY iinputid, imodelid) mp ON mtfci.imodelid = mp.imodelid AND mtfci.icompoundinputid = mp.iinputid " & _
                "GROUP BY mtfci.imodelid, mtfci.icardid, ci.iinputid "

            Else

                queryPreciosHistoricosInsumoCompuesto = "" & _
                "SELECT ptfci.iprojectid, ptfci.icardid, STR_TO_DATE(CONCAT(ptfci.iupdatedate, ' ', ptfci.supdatetime), '%Y%c%d %T') AS 'Fecha Hora', " & _
                "FORMAT(SUM(ptfci.dcompoundinputqty*pp.dinputfinalprice), 3) AS 'Precio' " & _
                "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs ptfci " & _
                "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompoundInput" & iinputid & " ci ON ci.icompoundinputid = ptfci.icompoundinputid " & _
                "LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " i ON i.iinputid = ptfci.icompoundinputid " & _
                "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, iprojectid) pp ON ptfci.iprojectid = pp.iprojectid AND ptfci.icompoundinputid = pp.iinputid " & _
                "GROUP BY ptfci.iprojectid, ptfci.icardid, ci.iinputid "

            End If

        End If



        Dim dtPreciosHistoricosInsumoCompuesto As DataTable
        dtPreciosHistoricosInsumoCompuesto = setDataGridView(dgvPreciosHistoricosInsumo, queryPreciosHistoricosInsumoCompuesto, True)

        dgvPreciosHistoricosInsumo.Columns(0).Visible = False
        dgvPreciosHistoricosInsumo.Columns(1).Visible = False


        'CODIGO PARA LA GRAFICA


        If IsHistoric = True Then

            btnGuardar.Visible = False

        End If

        executeSQLCommand(0, "INSERT INTO logs VALUES (" & getAppDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Abrió el Insumo Compuesto " & iinputid & ": " & txtNombreDelInsumo.Text & "', 'OK')")

        isFormReadyForAction = True
        isUnidadReady = True

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub txtNombreDelInsumo_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtNombreDelInsumo.KeyUp

        Dim strcaracteresprohibidos As String = "|@'\"""
        Dim arrayCaractProhib As Char() = strcaracteresprohibidos.ToCharArray
        Dim resultado As Boolean = False

        For carp = 0 To arrayCaractProhib.Length - 1

            If txtNombreDelInsumo.Text.Contains(arrayCaractProhib(carp)) Then
                txtNombreDelInsumo.Text = txtNombreDelInsumo.Text.Replace(arrayCaractProhib(carp), "")
                resultado = True
            End If

        Next carp

        txtNombreDelInsumo.Text = txtNombreDelInsumo.Text.Replace("--", "")

        If resultado = True Then
            txtNombreDelInsumo.Select(txtNombreDelInsumo.Text.Length, 0)
        End If

    End Sub


    Private Sub txtUnidadDeMedida_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtUnidadDeMedida.KeyUp

        Dim strcaracteresprohibidos As String = "|°!#$%&/()=?¡*¨[]_:;,-{}+´¿'¬^`~@\<>"
        Dim arrayCaractProhib As Char() = strcaracteresprohibidos.ToCharArray
        Dim resultado As Boolean = False

        For carp = 0 To arrayCaractProhib.Length - 1

            If txtUnidadDeMedida.Text.Contains(arrayCaractProhib(carp)) Then
                txtUnidadDeMedida.Text = txtUnidadDeMedida.Text.Replace(arrayCaractProhib(carp), "")
                resultado = True
            End If

        Next carp

        If resultado = True Then
            txtUnidadDeMedida.Select(txtUnidadDeMedida.Text.Length, 0)
        End If

    End Sub


    Private Sub txtUnidadDeMedida_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtUnidadDeMedida.LostFocus

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        If isFormReadyForAction = False Then
            Exit Sub
        End If

        If IsHistoric = True Then
            Exit Sub
        End If

        Dim unitfound As Boolean = False
        Dim strcaracteresprohibidos As String = "|°!#$%&/()=?¡*¨[]_:;,.-{}+´¿'¬^`~@\<> "

        txtUnidadDeMedida.Text = txtUnidadDeMedida.Text.Trim.Trim(strcaracteresprohibidos.ToCharArray)
        txtUnidadDeMedida.Text = txtUnidadDeMedida.Text.ToUpper

        unitfound = getSQLQueryAsBoolean(0, "SELECT count(*) FROM transformationunits WHERE soriginunit = '" & txtUnidadDeMedida.Text.Replace("--", "") & "'")

        If unitfound = False Then

            MsgBox("¡No encontré esa unidad de Medida! ¿Podrías seleccionar una de la lista?", MsgBoxStyle.OkOnly, "Unidad de Medida No Encontrada (Err 91)")

            Dim bu As New BuscaUnidades
            bu.susername = susername
            bu.bactive = bactive
            bu.bonline = bonline
            bu.suserfirstname = suserfirstname
            bu.suserlastname = suserlastname
            bu.suseremail = suseremail
            bu.susersession = susersession
            bu.susermachinename = susermachinename
            bu.suserip = suserip
            bu.permisoActivosCrear = permisoActivosCrear
            bu.permisoActivosEliminar = permisoActivosEliminar
            bu.permisoActivosModificar = permisoActivosModificar
            bu.permisoActivosVer = permisoActivosVer
            bu.permisoAnalisisDeUtilidadesCrear = permisoAnalisisDeUtilidadesCrear
            bu.permisoAnalisisDeUtilidadesEliminar = permisoAnalisisDeUtilidadesEliminar
            bu.permisoAnalisisDeUtilidadesModificar = permisoAnalisisDeUtilidadesModificar
            bu.permisoAnalisisDeUtilidadesVer = permisoAnalisisDeUtilidadesVer
            bu.permisoPersonasCrear = permisoPersonasCrear
            bu.permisoPersonasEliminar = permisoPersonasEliminar
            bu.permisoPersonasModificar = permisoPersonasModificar
            bu.permisoPersonasVer = permisoPersonasVer
            bu.permisoCotizacionesCrear = permisoCotizacionesCrear
            bu.permisoCotizacionesEliminar = permisoCotizacionesEliminar
            bu.permisoCotizacionesModificar = permisoCotizacionesModificar
            bu.permisoCotizacionesVer = permisoCotizacionesVer
            bu.permisoEnviosCrear = permisoEnviosCrear
            bu.permisoEnviosEliminar = permisoEnviosEliminar
            bu.permisoEnviosModificar = permisoEnviosModificar
            bu.permisoEnviosVer = permisoEnviosVer
            bu.permisoEquivalenciasCrear = permisoEquivalenciasCrear
            bu.permisoEquivalenciasEliminar = permisoEquivalenciasEliminar
            bu.permisoEquivalenciasModificar = permisoEquivalenciasModificar
            bu.permisoEquivalenciasVer = permisoEquivalenciasVer
            bu.permisoFacturasMRDCrear = permisoFacturasMRDCrear
            bu.permisoFacturasMRDEliminar = permisoFacturasMRDEliminar
            bu.permisoFacturasMRDModificar = permisoFacturasMRDModificar
            bu.permisoFacturasMRDVer = permisoFacturasMRDVer
            bu.permisoGastosAsignarAActivo = permisoGastosAsignarAActivo
            bu.permisoGastosAsignarAProyecto = permisoGastosAsignarAProyecto
            bu.permisoGastosCrear = permisoGastosCrear
            bu.permisoGastosEliminar = permisoGastosEliminar
            bu.permisoGastosModificar = permisoGastosModificar
            bu.permisoGastosVer = permisoGastosVer
            bu.permisoIngresosCrear = permisoIngresosCrear
            bu.permisoIngresosEliminar = permisoIngresosEliminar
            bu.permisoIngresosModificar = permisoIngresosModificar
            bu.permisoIngresosVer = permisoIngresosVer
            bu.permisoInsumosCrear = permisoInsumosCrear
            bu.permisoInsumosEliminar = permisoInsumosEliminar
            bu.permisoInsumosModificar = permisoInsumosModificar
            bu.permisoInsumosVer = permisoInsumosVer
            bu.permisoInventarioCrear = permisoInventarioCrear
            bu.permisoInventarioEliminar = permisoInventarioEliminar
            bu.permisoInventarioModificar = permisoInventarioModificar
            bu.permisoInventarioVer = permisoInventarioVer
            bu.permisoLogsEliminar = permisoLogsEliminar
            bu.permisoLogsVer = permisoLogsVer
            bu.permisoModelosCrear = permisoModelosCrear
            bu.permisoModelosEliminar = permisoModelosEliminar
            bu.permisoModelosModificar = permisoModelosModificar
            bu.permisoModelosPlanos = permisoModelosPlanos
            bu.permisoModelosVer = permisoModelosVer
            bu.permisoOrdenesCrear = permisoOrdenesCrear
            bu.permisoOrdenesEliminar = permisoOrdenesEliminar
            bu.permisoOrdenesModificar = permisoOrdenesModificar
            bu.permisoOrdenesVer = permisoOrdenesVer
            bu.permisoPersonasCrear = permisoPersonasCrear
            bu.permisoPersonasEliminar = permisoPersonasEliminar
            bu.permisoPersonasModificar = permisoPersonasModificar
            bu.permisoPersonasVer = permisoPersonasVer
            bu.permisoProveedoresCrear = permisoProveedoresCrear
            bu.permisoProveedoresEliminar = permisoProveedoresEliminar
            bu.permisoProveedoresModificar = permisoProveedoresModificar
            bu.permisoProveedoresVer = permisoProveedoresVer
            bu.permisoProyectosCrear = permisoProyectosCrear
            bu.permisoProyectosEliminar = permisoProyectosEliminar
            bu.permisoProyectosModificar = permisoProyectosModificar
            bu.permisoProyectosPlanos = permisoProyectosPlanos
            bu.permisoProyectosVer = permisoProyectosVer
            bu.permisoResumenDeTarjetasCrear = permisoResumenDeTarjetasCrear
            bu.permisoResumenDeTarjetasEliminar = permisoResumenDeTarjetasEliminar
            bu.permisoResumenDeTarjetasModificar = permisoResumenDeTarjetasModificar
            bu.permisoResumenDeTarjetasVer = permisoResumenDeTarjetasVer
            bu.permisoValesDeGasolinaCrear = permisoValesDeGasolinaCrear
            bu.permisoValesDeGasolinaEliminar = permisoValesDeGasolinaEliminar
            bu.permisoValesDeGasolinaModificar = permisoValesDeGasolinaModificar
            bu.permisoValesDeGasolinaVer = permisoValesDeGasolinaVer

            bu.querystring = txtUnidadDeMedida.Text

            bu.isEdit = False

            bu.ShowDialog(Me)

            If bu.DialogResult = Windows.Forms.DialogResult.OK Then

                txtUnidadDeMedida.Text = bu.sunit1

                If validaInsumo(True) = False Then
                    Cursor.Current = System.Windows.Forms.Cursors.Default
                    Exit Sub
                Else
                    dgvInsumos.Enabled = True
                End If

                Dim fecha As Integer = 0
                Dim hora As String = "00:00:00"

                fecha = getAppDate()
                hora = getAppTime()

                Dim baseid As Integer = 0
                baseid = getSQLQueryAsInteger(0, "SELECT ibaseid FROM base ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

                If baseid = 0 Then
                    baseid = 1
                End If

                If IsEdit = True Then

                    Dim queries(3) As String

                    queries(0) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " SET iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "', sinputdescription = '" & txtNombreDelInsumo.Text & "', sinputunit = '" & txtUnidadDeMedida.Text & "' WHERE iinputid = " & iinputid
                    queries(1) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types SET sinputtypedescription = '" & cmbTipoDeInsumo.SelectedItem.ToString & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE iinputid = " & iinputid

                    If IsBase = True Then
                        queries(2) = "INSERT INTO logs VALUES (" & getAppDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Presupuesto Base " & iprojectid & "', 'OK')"
                    Else
                        If IsModel = True Then
                            queries(2) = "INSERT INTO logs VALUES (" & getAppDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Modelo " & iprojectid & " : " & getSQLQueryAsString(0, "SELECT smodelname FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & " WHERE imodelid = " & iprojectid) & "', 'OK')"
                        Else
                            queries(2) = "INSERT INTO logs VALUES (" & getAppDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Proyecto " & iprojectid & " : " & getSQLQueryAsString(0, "SELECT sprojectname FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & " WHERE iprojectid = " & iprojectid) & "', 'OK')"
                        End If
                    End If

                    executeTransactedSQLCommand(0, queries)

                Else

                    Dim checkIfItsOnlyTextUpdate As Boolean = False

                    checkIfItsOnlyTextUpdate = getSQLQueryAsBoolean(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " WHERE iinputid = " & iinputid)

                    If checkIfItsOnlyTextUpdate = True Then

                        Dim queries(3) As String

                        queries(0) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " SET iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "', sinputdescription = '" & txtNombreDelInsumo.Text & "', sinputunit = '" & txtUnidadDeMedida.Text & "' WHERE iinputid = " & iinputid

                        If iinputid <> 0 Then
                            queries(1) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types SET sinputtypedescription = '" & cmbTipoDeInsumo.SelectedItem.ToString & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE iinputid = " & iinputid
                        End If

                        If IsBase = True Then
                            queries(2) = "INSERT INTO logs VALUES (" & getAppDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Presupuesto Base " & iprojectid & "', 'OK')"
                        Else
                            If IsModel = True Then
                                queries(2) = "INSERT INTO logs VALUES (" & getAppDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Modelo " & iprojectid & " : " & getSQLQueryAsString(0, "SELECT smodelname FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & " WHERE imodelid = " & iprojectid) & "', 'OK')"
                            Else
                                queries(2) = "INSERT INTO logs VALUES (" & getAppDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Proyecto " & iprojectid & " : " & getSQLQueryAsString(0, "SELECT sprojectname FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & " WHERE iprojectid = " & iprojectid) & "', 'OK')"
                            End If
                        End If

                        executeTransactedSQLCommand(0, queries)

                    Else

                        Dim queries(3) As String

                        iinputid = getSQLQueryAsInteger(0, "SELECT IF(MAX(iinputid) + 1 IS NULL, 1, MAX(iinputid) + 1) AS iinputid FROM inputs ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

                        Dim queriesCreation(9) As String

                        queriesCreation(0) = "DROP TABLE IF EXISTS oversight. tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input0"
                        queriesCreation(1) = "DROP TABLE IF EXISTS oversight. tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid
                        queriesCreation(2) = "CREATE TABLE  oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " ( `iinputid` int(11) NOT NULL AUTO_INCREMENT, `sinputdescription` varchar(300) CHARACTER SET latin1 NOT NULL, `sinputunit` varchar(100) CHARACTER SET latin1 NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL,  PRIMARY KEY (`iinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

                        queriesCreation(3) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompoundInput0"
                        queriesCreation(4) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompoundInput" & iinputid
                        queriesCreation(5) = "CREATE TABLE  oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompoundInput" & iinputid & " ( `iinputid` int(11) NOT NULL, `icompoundinputid` int(11) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`iinputid`,`icompoundinputid`) USING BTREE, KEY `inputid` (`iinputid`), KEY `compoundinputid` (`icompoundinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

                        queriesCreation(6) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input0Types"
                        queriesCreation(7) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types"
                        queriesCreation(8) = "CREATE TABLE  oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types" & " ( `iinputid` int(11) NOT NULL, `sinputtypedescription` varchar(250) CHARACTER SET latin1 NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`iinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci COMMENT='Only to differ which input is taken as Mano de Obra'"

                        executeTransactedSQLCommand(0, queriesCreation)

                        queries(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " VALUES (" & iinputid & ", '" & txtNombreDelInsumo.Text & "', '" & txtUnidadDeMedida.Text & "', " & fecha & ", '" & hora & "', '" & susername & "')"

                        queries(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types VALUES (" & iinputid & ", '" & cmbTipoDeInsumo.SelectedItem.ToString & "', " & fecha & ", '" & hora & "', '" & susername & "')"

                        If IsBase = True Then
                            queries(2) = "INSERT INTO logs VALUES (" & getAppDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Presupuesto Base " & iprojectid & "', 'OK')"
                        Else
                            If IsModel = True Then
                                queries(2) = "INSERT INTO logs VALUES (" & getAppDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Modelo " & iprojectid & " : " & getSQLQueryAsString(0, "SELECT smodelname FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & " WHERE imodelid = " & iprojectid) & "', 'OK')"
                            Else
                                queries(2) = "INSERT INTO logs VALUES (" & getAppDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Proyecto " & iprojectid & " : " & getSQLQueryAsString(0, "SELECT sprojectname FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & " WHERE iprojectid = " & iprojectid) & "', 'OK')"
                            End If
                        End If

                        executeTransactedSQLCommand(0, queries)

                    End If

                End If

            Else

                txtUnidadDeMedida.Focus()
                Exit Sub

            End If

        Else


            If validaInsumo(True) = False Then
                Cursor.Current = System.Windows.Forms.Cursors.Default
                Exit Sub
            Else
                dgvInsumos.Enabled = True
            End If

            Dim fecha As Integer = 0
            Dim hora As String = "00:00:00"

            fecha = getAppDate()
            hora = getAppTime()

            Dim baseid As Integer = 0
            baseid = getSQLQueryAsInteger(0, "SELECT ibaseid FROM base ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

            If baseid = 0 Then
                baseid = 1
            End If

            If IsEdit = True Then

                Dim queries(3) As String

                queries(0) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " SET iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "', sinputdescription = '" & txtNombreDelInsumo.Text & "', sinputunit = '" & txtUnidadDeMedida.Text & "' WHERE iinputid = " & iinputid
                queries(1) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types SET sinputtypedescription = '" & cmbTipoDeInsumo.SelectedItem.ToString & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE iinputid = " & iinputid

                If IsBase = True Then
                    queries(2) = "INSERT INTO logs VALUES (" & getAppDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Presupuesto Base " & iprojectid & "', 'OK')"
                Else
                    If IsModel = True Then
                        queries(2) = "INSERT INTO logs VALUES (" & getAppDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Modelo " & iprojectid & " : " & getSQLQueryAsString(0, "SELECT smodelname FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & " WHERE imodelid = " & iprojectid) & "', 'OK')"
                    Else
                        queries(2) = "INSERT INTO logs VALUES (" & getAppDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Proyecto " & iprojectid & " : " & getSQLQueryAsString(0, "SELECT sprojectname FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & " WHERE iprojectid = " & iprojectid) & "', 'OK')"
                    End If
                End If

                executeTransactedSQLCommand(0, queries)

            Else

                Dim checkIfItsOnlyTextUpdate As Boolean = False

                checkIfItsOnlyTextUpdate = getSQLQueryAsBoolean(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " WHERE iinputid = " & iinputid)

                If checkIfItsOnlyTextUpdate = True Then

                    Dim queries(3) As String

                    queries(0) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " SET iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "', sinputdescription = '" & txtNombreDelInsumo.Text & "', sinputunit = '" & txtUnidadDeMedida.Text & "' WHERE iinputid = " & iinputid

                    If iinputid <> 0 Then
                        queries(1) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types SET sinputtypedescription = '" & cmbTipoDeInsumo.SelectedItem.ToString & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE iinputid = " & iinputid
                    End If

                    If IsBase = True Then
                        queries(2) = "INSERT INTO logs VALUES (" & getAppDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Presupuesto Base " & iprojectid & "', 'OK')"
                    Else
                        If IsModel = True Then
                            queries(2) = "INSERT INTO logs VALUES (" & getAppDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Modelo " & iprojectid & " : " & getSQLQueryAsString(0, "SELECT smodelname FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & " WHERE imodelid = " & iprojectid) & "', 'OK')"
                        Else
                            queries(2) = "INSERT INTO logs VALUES (" & getAppDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Proyecto " & iprojectid & " : " & getSQLQueryAsString(0, "SELECT sprojectname FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & " WHERE iprojectid = " & iprojectid) & "', 'OK')"
                        End If
                    End If

                    executeTransactedSQLCommand(0, queries)

                Else

                    Dim queries(3) As String

                    iinputid = getSQLQueryAsInteger(0, "SELECT IF(MAX(iinputid) + 1 IS NULL, 1, MAX(iinputid) + 1) AS iinputid FROM inputs ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

                    Dim queriesCreation(9) As String

                    queriesCreation(0) = "DROP TABLE IF EXISTS oversight. tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input0"
                    queriesCreation(1) = "DROP TABLE IF EXISTS oversight. tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid
                    queriesCreation(2) = "CREATE TABLE  oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " ( `iinputid` int(11) NOT NULL AUTO_INCREMENT, `sinputdescription` varchar(300) CHARACTER SET latin1 NOT NULL, `sinputunit` varchar(100) CHARACTER SET latin1 NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL,  PRIMARY KEY (`iinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

                    queriesCreation(3) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompoundInput0"
                    queriesCreation(4) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompoundInput" & iinputid
                    queriesCreation(5) = "CREATE TABLE  oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompoundInput" & iinputid & " ( `iinputid` int(11) NOT NULL, `icompoundinputid` int(11) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`iinputid`,`icompoundinputid`) USING BTREE, KEY `inputid` (`iinputid`), KEY `compoundinputid` (`icompoundinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

                    queriesCreation(6) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input0Types"
                    queriesCreation(7) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types"
                    queriesCreation(8) = "CREATE TABLE  oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types" & " ( `iinputid` int(11) NOT NULL, `sinputtypedescription` varchar(250) CHARACTER SET latin1 NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`iinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci COMMENT='Only to differ which input is taken as Mano de Obra'"

                    executeTransactedSQLCommand(0, queriesCreation)

                    queries(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " VALUES (" & iinputid & ", '" & txtNombreDelInsumo.Text & "', '" & txtUnidadDeMedida.Text & "', " & fecha & ", '" & hora & "', '" & susername & "')"

                    queries(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types VALUES (" & iinputid & ", '" & cmbTipoDeInsumo.SelectedItem.ToString & "', " & fecha & ", '" & hora & "', '" & susername & "')"

                    If IsBase = True Then
                        queries(2) = "INSERT INTO logs VALUES (" & getAppDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Presupuesto Base " & iprojectid & "', 'OK')"
                    Else
                        If IsModel = True Then
                            queries(2) = "INSERT INTO logs VALUES (" & getAppDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Modelo " & iprojectid & " : " & getSQLQueryAsString(0, "SELECT smodelname FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & " WHERE imodelid = " & iprojectid) & "', 'OK')"
                        Else
                            queries(2) = "INSERT INTO logs VALUES (" & getAppDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Proyecto " & iprojectid & " : " & getSQLQueryAsString(0, "SELECT sprojectname FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & " WHERE iprojectid = " & iprojectid) & "', 'OK')"
                        End If
                    End If

                    executeTransactedSQLCommand(0, queries)

                End If

            End If

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Function validaInsumo(ByVal silent As Boolean) As Boolean

        Dim strcaracteresprohibidos As String = "|°!#$%&/()=?¡*¨[]_:;,-{}+´¿'¬^`~@\<>"
        Dim strcaracteresprohibidos2 As String = "|@'\"""

        txtUnidadDeMedida.Text = txtUnidadDeMedida.Text.Trim(strcaracteresprohibidos.ToCharArray)
        txtNombreDelInsumo.Text = txtNombreDelInsumo.Text.Trim(strcaracteresprohibidos2.ToCharArray).Replace("--", "")


        If txtNombreDelInsumo.Text = "" Then
            If silent = False Then
                MsgBox("¿Podrías poner una descripción a la Tarjeta?", MsgBoxStyle.OkOnly, "Dato Faltante (Err 76)")
            End If
            Return False
        End If


        If cmbTipoDeInsumo.SelectedIndex = -1 Then
            If silent = False Then
                MsgBox("¿Podrías escoger un Tipo de Insumo para este Insumo?", MsgBoxStyle.OkOnly, "Dato Faltante (Err 73)")
            End If
            Return False
        End If


        If txtUnidadDeMedida.Text = "" Then
            If silent = False Then
                MsgBox("¿Podrías poner una unidad de medida para el Insumo?", MsgBoxStyle.OkOnly, "Dato Faltante (Err 74)")
            End If
            Return False
        End If

        Return True


    End Function


    Private Function validaInsumosCompuestos(ByVal silent As Boolean) As Boolean

        Dim valorInsumoCompuesto As Double = 0.0
        Dim queryValorInsumoCompuesto As String

        If IsBase = True Then

            queryValorInsumoCompuesto = "" & _
            "SELECT SUM(btfci.dcompoundinputqty*bp.dinputfinalprice) AS precio " & _
            "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs btfci " & _
            "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompoundInput" & iinputid & " ci ON ci.icompoundinputid = btfci.icompoundinputid " & _
            "LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " i ON i.iinputid = btfci.icompoundinputid " & _
            "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfci.ibaseid = bp.ibaseid AND btfci.icompoundinputid = bp.iinputid " & _
            "WHERE btfci.ibaseid = " & iprojectid & " and btfci.icardid = " & icardid & " " & _
            "GROUP BY ci.iinputid "

        Else

            If IsModel = True Then

                queryValorInsumoCompuesto = "" & _
                "SELECT SUM(mtfci.dcompoundinputqty*mp.dinputfinalprice) AS precio " & _
                "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs mtfci " & _
                "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompoundInput" & iinputid & " ci ON ci.icompoundinputid = mtfci.icompoundinputid " & _
                "LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " i ON i.iinputid = mtfci.icompoundinputid " & _
                "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) mp GROUP BY iinputid, imodelid) mp ON mtfci.imodelid = mp.imodelid AND mtfci.icompoundinputid = mp.iinputid " & _
                "WHERE mtfci.imodelid = " & iprojectid & " and mtfci.icardid = " & icardid & " " & _
                "GROUP BY ci.iinputid "

            Else

                queryValorInsumoCompuesto = "" & _
                "SELECT SUM(ptfci.dcompoundinputqty*pp.dinputfinalprice) AS precio " & _
                "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs ptfci " & _
                "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompoundInput" & iinputid & " ci ON ci.icompoundinputid = ptfci.icompoundinputid " & _
                "LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " i ON i.iinputid = ptfci.icompoundinputid " & _
                "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, iprojectid) pp ON ptfci.iprojectid = pp.iprojectid AND ptfci.icompoundinputid = pp.iinputid " & _
                "WHERE ptfci.iprojectid = " & iprojectid & " and ptfci.icardid = " & icardid & " " & _
                "GROUP BY ci.iinputid "

            End If

        End If


        Try
            valorInsumoCompuesto = getSQLQueryAsDouble(0, queryValorInsumoCompuesto)
        Catch ex As Exception

        End Try


        If valorInsumoCompuesto = 0 Or valorInsumoCompuesto = 0.0 Then

            If silent = False Then


                MsgBox("¿Podrías poner algún Insumo para este Insumo Compuesto?", MsgBoxStyle.OkOnly, "Dato Faltante (Err 74)")


            End If

            Return False

        End If


        If validaInsumo(silent) = False Then
            Return False
        End If


        Return True

    End Function


    Private Sub dgvPreciosHistoricosInsumo_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvPreciosHistoricosInsumo.CellClick

        Try
            ihistoricprojectid = CInt(dgvPreciosHistoricosInsumo.Rows(e.RowIndex).Cells(0).Value())
            ihistoriccardid = CInt(dgvPreciosHistoricosInsumo.Rows(e.RowIndex).Cells(1).Value())
        Catch ex As Exception

        End Try

    End Sub


    Private Sub dgvPreciosHistoricosInsumo_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvPreciosHistoricosInsumo.CellContentClick

        Try
            ihistoricprojectid = CInt(dgvPreciosHistoricosInsumo.Rows(e.RowIndex).Cells(0).Value())
            ihistoriccardid = CInt(dgvPreciosHistoricosInsumo.Rows(e.RowIndex).Cells(1).Value())
        Catch ex As Exception

        End Try

    End Sub


    Private Sub dgvPreciosHistoricosInsumo_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvPreciosHistoricosInsumo.CellDoubleClick

        If IsHistoric = False Then

            Dim aic As New AgregarInsumoCompuesto2
            aic.susername = susername
            aic.bactive = bactive
            aic.bonline = bonline
            aic.suserfirstname = suserfirstname
            aic.suserlastname = suserlastname
            aic.suseremail = suseremail
            aic.susersession = susersession
            aic.susermachinename = susermachinename
            aic.suserip = suserip
            aic.permisoActivosCrear = permisoActivosCrear
            aic.permisoActivosEliminar = permisoActivosEliminar
            aic.permisoActivosModificar = permisoActivosModificar
            aic.permisoActivosVer = permisoActivosVer
            aic.permisoAnalisisDeUtilidadesCrear = permisoAnalisisDeUtilidadesCrear
            aic.permisoAnalisisDeUtilidadesEliminar = permisoAnalisisDeUtilidadesEliminar
            aic.permisoAnalisisDeUtilidadesModificar = permisoAnalisisDeUtilidadesModificar
            aic.permisoAnalisisDeUtilidadesVer = permisoAnalisisDeUtilidadesVer
            aic.permisoPersonasCrear = permisoPersonasCrear
            aic.permisoPersonasEliminar = permisoPersonasEliminar
            aic.permisoPersonasModificar = permisoPersonasModificar
            aic.permisoPersonasVer = permisoPersonasVer
            aic.permisoCotizacionesCrear = permisoCotizacionesCrear
            aic.permisoCotizacionesEliminar = permisoCotizacionesEliminar
            aic.permisoCotizacionesModificar = permisoCotizacionesModificar
            aic.permisoCotizacionesVer = permisoCotizacionesVer
            aic.permisoEnviosCrear = permisoEnviosCrear
            aic.permisoEnviosEliminar = permisoEnviosEliminar
            aic.permisoEnviosModificar = permisoEnviosModificar
            aic.permisoEnviosVer = permisoEnviosVer
            aic.permisoEquivalenciasCrear = permisoEquivalenciasCrear
            aic.permisoEquivalenciasEliminar = permisoEquivalenciasEliminar
            aic.permisoEquivalenciasModificar = permisoEquivalenciasModificar
            aic.permisoEquivalenciasVer = permisoEquivalenciasVer
            aic.permisoFacturasMRDCrear = permisoFacturasMRDCrear
            aic.permisoFacturasMRDEliminar = permisoFacturasMRDEliminar
            aic.permisoFacturasMRDModificar = permisoFacturasMRDModificar
            aic.permisoFacturasMRDVer = permisoFacturasMRDVer
            aic.permisoGastosAsignarAActivo = permisoGastosAsignarAActivo
            aic.permisoGastosAsignarAProyecto = permisoGastosAsignarAProyecto
            aic.permisoGastosCrear = permisoGastosCrear
            aic.permisoGastosEliminar = permisoGastosEliminar
            aic.permisoGastosModificar = permisoGastosModificar
            aic.permisoGastosVer = permisoGastosVer
            aic.permisoIngresosCrear = permisoIngresosCrear
            aic.permisoIngresosEliminar = permisoIngresosEliminar
            aic.permisoIngresosModificar = permisoIngresosModificar
            aic.permisoIngresosVer = permisoIngresosVer
            aic.permisoInsumosCrear = permisoInsumosCrear
            aic.permisoInsumosEliminar = permisoInsumosEliminar
            aic.permisoInsumosModificar = permisoInsumosModificar
            aic.permisoInsumosVer = permisoInsumosVer
            aic.permisoInventarioCrear = permisoInventarioCrear
            aic.permisoInventarioEliminar = permisoInventarioEliminar
            aic.permisoInventarioModificar = permisoInventarioModificar
            aic.permisoInventarioVer = permisoInventarioVer
            aic.permisoLogsEliminar = permisoLogsEliminar
            aic.permisoLogsVer = permisoLogsVer
            aic.permisoModelosCrear = permisoModelosCrear
            aic.permisoModelosEliminar = permisoModelosEliminar
            aic.permisoModelosModificar = permisoModelosModificar
            aic.permisoModelosPlanos = permisoModelosPlanos
            aic.permisoModelosVer = permisoModelosVer
            aic.permisoOrdenesCrear = permisoOrdenesCrear
            aic.permisoOrdenesEliminar = permisoOrdenesEliminar
            aic.permisoOrdenesModificar = permisoOrdenesModificar
            aic.permisoOrdenesVer = permisoOrdenesVer
            aic.permisoPersonasCrear = permisoPersonasCrear
            aic.permisoPersonasEliminar = permisoPersonasEliminar
            aic.permisoPersonasModificar = permisoPersonasModificar
            aic.permisoPersonasVer = permisoPersonasVer
            aic.permisoProveedoresCrear = permisoProveedoresCrear
            aic.permisoProveedoresEliminar = permisoProveedoresEliminar
            aic.permisoProveedoresModificar = permisoProveedoresModificar
            aic.permisoProveedoresVer = permisoProveedoresVer
            aic.permisoProyectosCrear = permisoProyectosCrear
            aic.permisoProyectosEliminar = permisoProyectosEliminar
            aic.permisoProyectosModificar = permisoProyectosModificar
            aic.permisoProyectosPlanos = permisoProyectosPlanos
            aic.permisoProyectosVer = permisoProyectosVer
            aic.permisoResumenDeTarjetasCrear = permisoResumenDeTarjetasCrear
            aic.permisoResumenDeTarjetasEliminar = permisoResumenDeTarjetasEliminar
            aic.permisoResumenDeTarjetasModificar = permisoResumenDeTarjetasModificar
            aic.permisoResumenDeTarjetasVer = permisoResumenDeTarjetasVer
            aic.permisoValesDeGasolinaCrear = permisoValesDeGasolinaCrear
            aic.permisoValesDeGasolinaEliminar = permisoValesDeGasolinaEliminar
            aic.permisoValesDeGasolinaModificar = permisoValesDeGasolinaModificar
            aic.permisoValesDeGasolinaVer = permisoValesDeGasolinaVer

            aic.iprojectid = ihistoricprojectid
            aic.icardid = ihistoriccardid
            aic.iinputid = iinputid

            aic.IsEdit = True
            aic.IsHistoric = True
            aic.IsModel = IsModel
            aic.IsBase = IsBase

            aic.ShowDialog(Me)

        End If

    End Sub


    Private Sub dgvPreciosHistoricosInsumo_CellContentDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvPreciosHistoricosInsumo.CellContentDoubleClick

        If IsHistoric = False Then

            Dim aic As New AgregarInsumo
            aic.susername = susername
            aic.bactive = bactive
            aic.bonline = bonline
            aic.suserfirstname = suserfirstname
            aic.suserlastname = suserlastname
            aic.suseremail = suseremail
            aic.susersession = susersession
            aic.susermachinename = susermachinename
            aic.suserip = suserip
            aic.permisoActivosCrear = permisoActivosCrear
            aic.permisoActivosEliminar = permisoActivosEliminar
            aic.permisoActivosModificar = permisoActivosModificar
            aic.permisoActivosVer = permisoActivosVer
            aic.permisoAnalisisDeUtilidadesCrear = permisoAnalisisDeUtilidadesCrear
            aic.permisoAnalisisDeUtilidadesEliminar = permisoAnalisisDeUtilidadesEliminar
            aic.permisoAnalisisDeUtilidadesModificar = permisoAnalisisDeUtilidadesModificar
            aic.permisoAnalisisDeUtilidadesVer = permisoAnalisisDeUtilidadesVer
            aic.permisoPersonasCrear = permisoPersonasCrear
            aic.permisoPersonasEliminar = permisoPersonasEliminar
            aic.permisoPersonasModificar = permisoPersonasModificar
            aic.permisoPersonasVer = permisoPersonasVer
            aic.permisoCotizacionesCrear = permisoCotizacionesCrear
            aic.permisoCotizacionesEliminar = permisoCotizacionesEliminar
            aic.permisoCotizacionesModificar = permisoCotizacionesModificar
            aic.permisoCotizacionesVer = permisoCotizacionesVer
            aic.permisoEnviosCrear = permisoEnviosCrear
            aic.permisoEnviosEliminar = permisoEnviosEliminar
            aic.permisoEnviosModificar = permisoEnviosModificar
            aic.permisoEnviosVer = permisoEnviosVer
            aic.permisoEquivalenciasCrear = permisoEquivalenciasCrear
            aic.permisoEquivalenciasEliminar = permisoEquivalenciasEliminar
            aic.permisoEquivalenciasModificar = permisoEquivalenciasModificar
            aic.permisoEquivalenciasVer = permisoEquivalenciasVer
            aic.permisoFacturasMRDCrear = permisoFacturasMRDCrear
            aic.permisoFacturasMRDEliminar = permisoFacturasMRDEliminar
            aic.permisoFacturasMRDModificar = permisoFacturasMRDModificar
            aic.permisoFacturasMRDVer = permisoFacturasMRDVer
            aic.permisoGastosAsignarAActivo = permisoGastosAsignarAActivo
            aic.permisoGastosAsignarAProyecto = permisoGastosAsignarAProyecto
            aic.permisoGastosCrear = permisoGastosCrear
            aic.permisoGastosEliminar = permisoGastosEliminar
            aic.permisoGastosModificar = permisoGastosModificar
            aic.permisoGastosVer = permisoGastosVer
            aic.permisoIngresosCrear = permisoIngresosCrear
            aic.permisoIngresosEliminar = permisoIngresosEliminar
            aic.permisoIngresosModificar = permisoIngresosModificar
            aic.permisoIngresosVer = permisoIngresosVer
            aic.permisoInsumosCrear = permisoInsumosCrear
            aic.permisoInsumosEliminar = permisoInsumosEliminar
            aic.permisoInsumosModificar = permisoInsumosModificar
            aic.permisoInsumosVer = permisoInsumosVer
            aic.permisoInventarioCrear = permisoInventarioCrear
            aic.permisoInventarioEliminar = permisoInventarioEliminar
            aic.permisoInventarioModificar = permisoInventarioModificar
            aic.permisoInventarioVer = permisoInventarioVer
            aic.permisoLogsEliminar = permisoLogsEliminar
            aic.permisoLogsVer = permisoLogsVer
            aic.permisoModelosCrear = permisoModelosCrear
            aic.permisoModelosEliminar = permisoModelosEliminar
            aic.permisoModelosModificar = permisoModelosModificar
            aic.permisoModelosPlanos = permisoModelosPlanos
            aic.permisoModelosVer = permisoModelosVer
            aic.permisoOrdenesCrear = permisoOrdenesCrear
            aic.permisoOrdenesEliminar = permisoOrdenesEliminar
            aic.permisoOrdenesModificar = permisoOrdenesModificar
            aic.permisoOrdenesVer = permisoOrdenesVer
            aic.permisoPersonasCrear = permisoPersonasCrear
            aic.permisoPersonasEliminar = permisoPersonasEliminar
            aic.permisoPersonasModificar = permisoPersonasModificar
            aic.permisoPersonasVer = permisoPersonasVer
            aic.permisoProveedoresCrear = permisoProveedoresCrear
            aic.permisoProveedoresEliminar = permisoProveedoresEliminar
            aic.permisoProveedoresModificar = permisoProveedoresModificar
            aic.permisoProveedoresVer = permisoProveedoresVer
            aic.permisoProyectosCrear = permisoProyectosCrear
            aic.permisoProyectosEliminar = permisoProyectosEliminar
            aic.permisoProyectosModificar = permisoProyectosModificar
            aic.permisoProyectosPlanos = permisoProyectosPlanos
            aic.permisoProyectosVer = permisoProyectosVer
            aic.permisoResumenDeTarjetasCrear = permisoResumenDeTarjetasCrear
            aic.permisoResumenDeTarjetasEliminar = permisoResumenDeTarjetasEliminar
            aic.permisoResumenDeTarjetasModificar = permisoResumenDeTarjetasModificar
            aic.permisoResumenDeTarjetasVer = permisoResumenDeTarjetasVer
            aic.permisoValesDeGasolinaCrear = permisoValesDeGasolinaCrear
            aic.permisoValesDeGasolinaEliminar = permisoValesDeGasolinaEliminar
            aic.permisoValesDeGasolinaModificar = permisoValesDeGasolinaModificar
            aic.permisoValesDeGasolinaVer = permisoValesDeGasolinaVer

            aic.iprojectid = ihistoricprojectid
            aic.icardid = ihistoriccardid
            aic.iinputid = iinputid

            aic.IsEdit = True
            aic.IsHistoric = True
            aic.IsModel = IsModel
            aic.IsBase = IsBase

            aic.ShowDialog(Me)

        End If

    End Sub


    Private Sub dgvInsumos_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvInsumos.CellClick

        If validaInsumo(True) = False Then
            Exit Sub
        End If

        If dgvInsumos.CurrentRow.IsNewRow Then
            Exit Sub
        End If

        Try
            iselectedinputid = CInt(dgvInsumos.Rows(e.RowIndex).Cells(0).Value())
            sselectedinputdescription = dgvInsumos.Rows(e.RowIndex).Cells(1).Value()
            sselectedunit = dgvInsumos.Rows(e.RowIndex).Cells(2).Value()
            dselectedinputqty = CDbl(dgvInsumos.Rows(e.RowIndex).Cells(3).Value())
        Catch ex As Exception

        End Try

    End Sub


    Private Sub dgvInsumos_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvInsumos.CellContentClick

        If validaInsumo(True) = False Then
            Exit Sub
        End If

        If dgvInsumos.CurrentRow.IsNewRow Then
            Exit Sub
        End If

        Try
            iselectedinputid = CInt(dgvInsumos.Rows(e.RowIndex).Cells(0).Value())
            sselectedinputdescription = dgvInsumos.Rows(e.RowIndex).Cells(1).Value()
            sselectedunit = dgvInsumos.Rows(e.RowIndex).Cells(2).Value()
            dselectedinputqty = CDbl(dgvInsumos.Rows(e.RowIndex).Cells(3).Value())
        Catch ex As Exception

        End Try

    End Sub


    Private Sub dgvInsumos_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgvInsumos.SelectionChanged

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        txtNombreDgvInsumos = Nothing
        txtNombreDgvInsumos_OldText = Nothing
        txtCantidadDgvInsumos = Nothing
        txtCantidadDgvInsumos_OldText = Nothing

        If isFormReadyForAction = False Then
            Exit Sub
        End If

        If dgvInsumos.CurrentRow.IsNewRow Then
            Exit Sub
        End If

        Try
            iselectedinputid = CInt(dgvInsumos.CurrentRow.Cells(0).Value)
            sselectedinputdescription = dgvInsumos.CurrentRow.Cells(1).Value()
            sselectedunit = dgvInsumos.CurrentRow.Cells(2).Value()
            dselectedinputqty = CDbl(dgvInsumos.CurrentRow.Cells(3).Value)
        Catch ex As Exception

        End Try

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub dgvInsumos_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvInsumos.CellEndEdit

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim queryInsumos As String

        If IsBase = True Then

            queryInsumos = "" & _
            "SELECT btfci.icompoundinputid, i.sinputdescription AS 'Insumo', btfci.scompoundinputunit AS 'Unidad', btfci.dcompoundinputqty AS 'Cantidad', " & _
            "FORMAT((btfci.dcompoundinputqty*bp.dinputfinalprice), 3) AS 'Precio' " & _
            "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs btfci " & _
            "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompoundInput" & iinputid & " ci ON ci.icompoundinputid = btfci.icompoundinputid " & _
            "LEFT JOIN inputs i ON i.iinputid = btfci.icompoundinputid " & _
            "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfci.ibaseid = bp.ibaseid AND btfci.icompoundinputid = bp.iinputid " & _
            "WHERE btfci.ibaseid = " & iprojectid & " AND btfci.icardid = " & icardid & " AND ci.iinputid = " & iinputid

        Else

            If IsModel = True Then

                queryInsumos = "" & _
                "SELECT mtfci.icompoundinputid, i.sinputdescription AS 'Insumo', mtfci.scompoundinputunit AS 'Unidad', mtfci.dcompoundinputqty AS 'Cantidad', " & _
                "FORMAT((mtfci.dcompoundinputqty*mp.dinputfinalprice), 3) AS 'Precio' " & _
                "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs mtfci " & _
                "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompoundInput" & iinputid & " ci ON ci.icompoundinputid = mtfci.icompoundinputid " & _
                "LEFT JOIN inputs i ON i.iinputid = mtfci.icompoundinputid " & _
                "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) mp GROUP BY iinputid, imodelid) mp ON mtfci.imodelid = mp.imodelid AND mtfci.icompoundinputid = mp.iinputid " & _
                "WHERE mtfci.imodelid = " & iprojectid & " AND mtfci.icardid = " & icardid & " AND ci.iinputid = " & iinputid

            Else

                queryInsumos = "" & _
                "SELECT ptfci.icompoundinputid, i.sinputdescription AS 'Insumo', ptfci.scompoundinputunit AS 'Unidad', FORMAT(ptfci.dcompoundinputqty, 3) AS 'Cantidad', " & _
                "FORMAT((ptfci.dcompoundinputqty*pp.dinputfinalprice), 3) AS 'Precio' " & _
                "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs ptfci " & _
                "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompoundInput" & iinputid & " ci ON ci.icompoundinputid = ptfci.icompoundinputid " & _
                "LEFT JOIN inputs i ON i.iinputid = ptfci.icompoundinputid " & _
                "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, iprojectid) pp ON ptfci.iprojectid = pp.iprojectid AND ptfci.icompoundinputid = pp.iinputid " & _
                "WHERE ptfci.iprojectid = " & iprojectid & " AND ptfci.icardid = " & icardid & " AND ci.iinputid = " & iinputid

            End If

        End If

        setDataGridView(dgvInsumos, queryInsumos, False)

        dgvInsumos.Columns(0).Visible = False

        dgvInsumos.Columns(0).ReadOnly = True
        dgvInsumos.Columns(2).ReadOnly = True
        dgvInsumos.Columns(4).ReadOnly = True

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub dgvInsumos_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvInsumos.CellValueChanged

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        'LAS UNICAS COLUMNAS EDITABLES SON LAS COLUMNAS 1 y 3: sinputdescription Y dcompoundinputqty

        If isFormReadyForAction = False Then
            Exit Sub
        End If

        Dim baseid As Integer = 0
        baseid = getSQLQueryAsInteger(0, "SELECT ibaseid FROM base ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

        If baseid = 0 Then
            baseid = 1
        End If

        Dim fecha As Integer = 0
        Dim hora As String = ""

        fecha = getAppDate()
        hora = getAppTime()

        If e.ColumnIndex = 1 Then

            'sinputdescription
            If dgvInsumos.Rows(e.RowIndex).Cells(e.ColumnIndex).Value Is DBNull.Value Then

                If MsgBox("¿Estás seguro de que quieres eliminar este Insumo del Insumo Compuesto?", MsgBoxStyle.YesNo, "Confirmación de Eliminación de Insumo de Insumo Compuesto (Err 36)") = MsgBoxResult.Yes Then

                    If IsBase = True Then

                        executeSQLCommand(0, "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs WHERE ibaseid = " & baseid & " AND icardid = " & icardid & " AND icompoundinputid = " & iselectedinputid)

                    Else

                        If IsModel = True Then

                            executeSQLCommand(0, "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs WHERE imodelid = " & iprojectid & " AND icardid = " & icardid & " AND icompoundinputid = " & iselectedinputid)

                        Else

                            executeSQLCommand(0, "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs WHERE iprojectid = " & iprojectid & " AND icardid = " & icardid & " AND icompoundinputid = " & iselectedinputid)

                        End If

                    End If


                    Dim querySumaInsumoCompuesto As String
                    Dim sumaInsumoCompuesto As Double

                    If IsBase = True Then

                        querySumaInsumoCompuesto = "" & _
                        "SELECT SUM(btfci.dcompoundinputqty*bp.dinputfinalprice) AS precio " & _
                        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs btfci " & _
                        "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompoundInput" & iinputid & " ci ON ci.icompoundinputid = btfci.icompoundinputid " & _
                        "LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " i ON i.iinputid = btfci.icompoundinputid " & _
                        "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfci.ibaseid = bp.ibaseid AND btfci.icompoundinputid = bp.iinputid " & _
                        "WHERE btfci.ibaseid = " & iprojectid & " and btfci.icardid = " & icardid & " " & _
                        "GROUP BY ci.iinputid "

                    Else

                        If IsModel = True Then

                            querySumaInsumoCompuesto = "" & _
                            "SELECT SUM(mtfci.dcompoundinputqty*mp.dinputfinalprice) AS precio " & _
                            "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs mtfci " & _
                            "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompoundInput" & iinputid & " ci ON ci.icompoundinputid = mtfci.icompoundinputid " & _
                            "LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " i ON i.iinputid = mtfci.icompoundinputid " & _
                            "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) mp GROUP BY iinputid, imodelid) mp ON mtfci.imodelid = mp.imodelid AND mtfci.icompoundinputid = mp.iinputid " & _
                            "WHERE mtfci.imodelid = " & iprojectid & " and mtfci.icardid = " & icardid & " " & _
                            "GROUP BY ci.iinputid "

                        Else

                            querySumaInsumoCompuesto = "" & _
                            "SELECT SUM(ptfci.dcompoundinputqty*pp.dinputfinalprice) AS precio " & _
                            "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs ptfci " & _
                            "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompoundInput" & iinputid & " ci ON ci.icompoundinputid = ptfci.icompoundinputid " & _
                            "LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " i ON i.iinputid = ptfci.icompoundinputid " & _
                            "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, iprojectid) pp ON ptfci.iprojectid = pp.iprojectid AND ptfci.icompoundinputid = pp.iinputid " & _
                            "WHERE ptfci.iprojectid = " & iprojectid & " and ptfci.icardid = " & icardid & " " & _
                            "GROUP BY ci.iinputid "

                        End If

                    End If

                    sumaInsumoCompuesto = getSQLQueryAsDouble(0, querySumaInsumoCompuesto)

                    txtCostoParaTabulador.Text = FormatCurrency(sumaInsumoCompuesto, 2, TriState.True, TriState.False, TriState.True)

                Else

                    dgvInsumos.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = getSQLQueryAsString(0, "SELECT sinputdescription FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " WHERE iinputid = " & iselectedinputid)

                End If

            Else

                'Si pone un texto, e.g. una descripcion de otro producto

                dgvInsumos.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = dgvInsumos.Rows(e.RowIndex).Cells(e.ColumnIndex).Value.ToString.Trim.Replace("'", "").Replace("--", "").Replace("@", "")

                Dim bi As New BuscaInsumos
                bi.susername = susername
                bi.bactive = bactive
                bi.bonline = bonline
                bi.suserfirstname = suserfirstname
                bi.suserlastname = suserlastname
                bi.suseremail = suseremail
                bi.susersession = susersession
                bi.susermachinename = susermachinename
                bi.suserip = suserip
                bi.permisoActivosCrear = permisoActivosCrear
                bi.permisoActivosEliminar = permisoActivosEliminar
                bi.permisoActivosModificar = permisoActivosModificar
                bi.permisoActivosVer = permisoActivosVer
                bi.permisoAnalisisDeUtilidadesCrear = permisoAnalisisDeUtilidadesCrear
                bi.permisoAnalisisDeUtilidadesEliminar = permisoAnalisisDeUtilidadesEliminar
                bi.permisoAnalisisDeUtilidadesModificar = permisoAnalisisDeUtilidadesModificar
                bi.permisoAnalisisDeUtilidadesVer = permisoAnalisisDeUtilidadesVer
                bi.permisoPersonasCrear = permisoPersonasCrear
                bi.permisoPersonasEliminar = permisoPersonasEliminar
                bi.permisoPersonasModificar = permisoPersonasModificar
                bi.permisoPersonasVer = permisoPersonasVer
                bi.permisoCotizacionesCrear = permisoCotizacionesCrear
                bi.permisoCotizacionesEliminar = permisoCotizacionesEliminar
                bi.permisoCotizacionesModificar = permisoCotizacionesModificar
                bi.permisoCotizacionesVer = permisoCotizacionesVer
                bi.permisoEnviosCrear = permisoEnviosCrear
                bi.permisoEnviosEliminar = permisoEnviosEliminar
                bi.permisoEnviosModificar = permisoEnviosModificar
                bi.permisoEnviosVer = permisoEnviosVer
                bi.permisoEquivalenciasCrear = permisoEquivalenciasCrear
                bi.permisoEquivalenciasEliminar = permisoEquivalenciasEliminar
                bi.permisoEquivalenciasModificar = permisoEquivalenciasModificar
                bi.permisoEquivalenciasVer = permisoEquivalenciasVer
                bi.permisoFacturasMRDCrear = permisoFacturasMRDCrear
                bi.permisoFacturasMRDEliminar = permisoFacturasMRDEliminar
                bi.permisoFacturasMRDModificar = permisoFacturasMRDModificar
                bi.permisoFacturasMRDVer = permisoFacturasMRDVer
                bi.permisoGastosAsignarAActivo = permisoGastosAsignarAActivo
                bi.permisoGastosAsignarAProyecto = permisoGastosAsignarAProyecto
                bi.permisoGastosCrear = permisoGastosCrear
                bi.permisoGastosEliminar = permisoGastosEliminar
                bi.permisoGastosModificar = permisoGastosModificar
                bi.permisoGastosVer = permisoGastosVer
                bi.permisoIngresosCrear = permisoIngresosCrear
                bi.permisoIngresosEliminar = permisoIngresosEliminar
                bi.permisoIngresosModificar = permisoIngresosModificar
                bi.permisoIngresosVer = permisoIngresosVer
                bi.permisoInsumosCrear = permisoInsumosCrear
                bi.permisoInsumosEliminar = permisoInsumosEliminar
                bi.permisoInsumosModificar = permisoInsumosModificar
                bi.permisoInsumosVer = permisoInsumosVer
                bi.permisoInventarioCrear = permisoInventarioCrear
                bi.permisoInventarioEliminar = permisoInventarioEliminar
                bi.permisoInventarioModificar = permisoInventarioModificar
                bi.permisoInventarioVer = permisoInventarioVer
                bi.permisoLogsEliminar = permisoLogsEliminar
                bi.permisoLogsVer = permisoLogsVer
                bi.permisoModelosCrear = permisoModelosCrear
                bi.permisoModelosEliminar = permisoModelosEliminar
                bi.permisoModelosModificar = permisoModelosModificar
                bi.permisoModelosPlanos = permisoModelosPlanos
                bi.permisoModelosVer = permisoModelosVer
                bi.permisoOrdenesCrear = permisoOrdenesCrear
                bi.permisoOrdenesEliminar = permisoOrdenesEliminar
                bi.permisoOrdenesModificar = permisoOrdenesModificar
                bi.permisoOrdenesVer = permisoOrdenesVer
                bi.permisoPersonasCrear = permisoPersonasCrear
                bi.permisoPersonasEliminar = permisoPersonasEliminar
                bi.permisoPersonasModificar = permisoPersonasModificar
                bi.permisoPersonasVer = permisoPersonasVer
                bi.permisoProveedoresCrear = permisoProveedoresCrear
                bi.permisoProveedoresEliminar = permisoProveedoresEliminar
                bi.permisoProveedoresModificar = permisoProveedoresModificar
                bi.permisoProveedoresVer = permisoProveedoresVer
                bi.permisoProyectosCrear = permisoProyectosCrear
                bi.permisoProyectosEliminar = permisoProyectosEliminar
                bi.permisoProyectosModificar = permisoProyectosModificar
                bi.permisoProyectosPlanos = permisoProyectosPlanos
                bi.permisoProyectosVer = permisoProyectosVer
                bi.permisoResumenDeTarjetasCrear = permisoResumenDeTarjetasCrear
                bi.permisoResumenDeTarjetasEliminar = permisoResumenDeTarjetasEliminar
                bi.permisoResumenDeTarjetasModificar = permisoResumenDeTarjetasModificar
                bi.permisoResumenDeTarjetasVer = permisoResumenDeTarjetasVer
                bi.permisoValesDeGasolinaCrear = permisoValesDeGasolinaCrear
                bi.permisoValesDeGasolinaEliminar = permisoValesDeGasolinaEliminar
                bi.permisoValesDeGasolinaModificar = permisoValesDeGasolinaModificar
                bi.permisoValesDeGasolinaVer = permisoValesDeGasolinaVer

                bi.querystring = dgvInsumos.CurrentCell.EditedFormattedValue

                bi.IsEdit = False

                bi.IsBase = IsBase
                bi.IsModel = IsModel

                bi.ShowDialog(Me)

                If bi.DialogResult = Windows.Forms.DialogResult.OK Then

                    Dim dsBusquedaInsumosRepetidos As DataSet

                    If IsBase = True Then

                        dsBusquedaInsumosRepetidos = getSQLQueryAsDataset(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs WHERE ibaseid = " & baseid & " AND icardid = " & icardid & " AND icompoundinputid = " & bi.iinputid)

                    Else

                        If IsModel = True Then

                            dsBusquedaInsumosRepetidos = getSQLQueryAsDataset(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs WHERE imodelid = " & iprojectid & " AND icardid = " & icardid & " AND icompoundinputid = " & bi.iinputid)

                        Else

                            dsBusquedaInsumosRepetidos = getSQLQueryAsDataset(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs WHERE iprojectid = " & iprojectid & " AND icardid = " & icardid & " AND icompoundinputid = " & bi.iinputid)

                        End If

                    End If


                    If dsBusquedaInsumosRepetidos.Tables(0).Rows.Count > 0 Then

                        MsgBox("Ya tienes ese Insumo insertado en este Insumo Compuesto. ¿Podrías buscarlo en la lista y cambiar la cantidad si así lo deseas?", MsgBoxStyle.OkOnly, "Insumo Repetido (Err 83)")
                        dgvInsumos.EndEdit()
                        Exit Sub

                    End If


                    If MsgBox("¿Estás seguro de que deseas reemplazar el Insumo " & dgvInsumos.Rows(e.RowIndex).Cells(e.ColumnIndex).Value & "?", MsgBoxStyle.YesNo, "Confirmación de Reemplazo de Insumo (Err 33)") = MsgBoxResult.Yes Then

                        Dim cantidaddeinsumo As Double = 1

                        Try
                            cantidaddeinsumo = CDbl(dgvInsumos.Rows(e.RowIndex).Cells(3).Value)
                        Catch ex As Exception

                        End Try

                        If IsBase = True Then

                            Dim queries(3) As String

                            queries(0) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs WHERE ibaseid = " & baseid & " AND icardid = " & icardid & " AND icompoundinputid = " & iselectedinputid
                            queries(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompoundInput" & iinputid & " VALUES (" & iinputid & ", " & bi.iinputid & ", " & fecha & ", '" & hora & "', '" & susername & "')"
                            queries(2) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs SELECT " & baseid & ", " & icardid & ", " & bi.iinputid & ", sinputunit, 1, " & fecha & ", '" & hora & "', '" & susername & "' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " WHERE iinputid = " & iinputid

                            executeTransactedSQLCommand(0, queries)

                        Else

                            If IsModel = True Then

                                Dim queries(3) As String

                                queries(0) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs WHERE imodelid = " & iprojectid & " AND icardid = " & icardid & " AND icompoundinputid = " & iselectedinputid
                                queries(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompoundInput" & iinputid & " VALUES (" & iinputid & ", " & bi.iinputid & ", " & fecha & ", '" & hora & "', '" & susername & "')"
                                queries(2) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs SELECT " & iprojectid & ", " & icardid & ", " & bi.iinputid & ", sinputunit, 1, " & fecha & ", '" & hora & "', '" & susername & "' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " WHERE iinputid = " & iinputid

                                executeTransactedSQLCommand(0, queries)

                            Else

                                Dim queries(3) As String

                                queries(0) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs WHERE iprojectid = " & iprojectid & " AND icardid = " & icardid & " AND icompoundinputid = " & iselectedinputid
                                queries(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompoundInput" & iinputid & " VALUES (" & iinputid & ", " & bi.iinputid & ", " & fecha & ", '" & hora & "', '" & susername & "')"
                                queries(2) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs SELECT " & iprojectid & ", " & icardid & ", " & bi.iinputid & ", sinputunit, 1, " & fecha & ", '" & hora & "', '" & susername & "' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " WHERE iinputid = " & iinputid

                                executeTransactedSQLCommand(0, queries)

                            End If

                        End If

                        Dim querySumaInsumoCompuesto As String
                        Dim sumaInsumoCompuesto As Double

                        If IsBase = True Then

                            querySumaInsumoCompuesto = "" & _
                            "SELECT SUM(btfci.dcompoundinputqty*bp.dinputfinalprice) AS precio " & _
                            "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs btfci " & _
                            "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompoundInput" & iinputid & " ci ON ci.icompoundinputid = btfci.icompoundinputid " & _
                            "LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " i ON i.iinputid = btfci.icompoundinputid " & _
                            "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfci.ibaseid = bp.ibaseid AND btfci.icompoundinputid = bp.iinputid " & _
                            "WHERE btfci.ibaseid = " & iprojectid & " and btfci.icardid = " & icardid & " " & _
                            "GROUP BY ci.iinputid "

                        Else

                            If IsModel = True Then

                                querySumaInsumoCompuesto = "" & _
                                "SELECT SUM(mtfci.dcompoundinputqty*mp.dinputfinalprice) AS precio " & _
                                "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs mtfci " & _
                                "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompoundInput" & iinputid & " ci ON ci.icompoundinputid = mtfci.icompoundinputid " & _
                                "LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " i ON i.iinputid = mtfci.icompoundinputid " & _
                                "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) mp GROUP BY iinputid, imodelid) mp ON mtfci.imodelid = mp.imodelid AND mtfci.icompoundinputid = mp.iinputid " & _
                                "WHERE mtfci.imodelid = " & iprojectid & " and mtfci.icardid = " & icardid & " " & _
                                "GROUP BY ci.iinputid "

                            Else

                                querySumaInsumoCompuesto = "" & _
                                "SELECT SUM(ptfci.dcompoundinputqty*pp.dinputfinalprice) AS precio " & _
                                "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs ptfci " & _
                                "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompoundInput" & iinputid & " ci ON ci.icompoundinputid = ptfci.icompoundinputid " & _
                                "LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " i ON i.iinputid = ptfci.icompoundinputid " & _
                                "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, iprojectid) pp ON ptfci.iprojectid = pp.iprojectid AND ptfci.icompoundinputid = pp.iinputid " & _
                                "WHERE ptfci.iprojectid = " & iprojectid & " and ptfci.icardid = " & icardid & " " & _
                                "GROUP BY ci.iinputid "

                            End If

                        End If

                        sumaInsumoCompuesto = getSQLQueryAsDouble(0, querySumaInsumoCompuesto)

                        txtCostoParaTabulador.Text = FormatCurrency(sumaInsumoCompuesto, 2, TriState.True, TriState.False, TriState.True)

                    Else

                        'Si cancela el reemplazo de insumo
                        dgvInsumos.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = sselectedinputdescription

                    End If


                Else

                    'Si cancela el reemplazo de insumo
                    dgvInsumos.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = sselectedinputdescription

                End If

            End If

        ElseIf e.ColumnIndex = 3 Then

            'dcompoundinputqty

            If dgvInsumos.Rows(e.RowIndex).Cells(e.ColumnIndex).Value Is DBNull.Value Then

                If MsgBox("¿Estás seguro de que quieres eliminar este Insumo del Insumo Compuesto?", MsgBoxStyle.YesNo, "Confirmación de Eliminación de Insumo de Insumo Compuesto (Err 36)") = MsgBoxResult.Yes Then

                    If IsBase = True Then

                        executeSQLCommand(0, "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs WHERE ibaseid = " & baseid & " AND icardid = " & icardid & " AND icompoundinputid = " & iselectedinputid)

                    Else

                        If IsModel = True Then

                            executeSQLCommand(0, "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs WHERE imodelid = " & iprojectid & " AND icardid = " & icardid & " AND icompoundinputid = " & iselectedinputid)

                        Else

                            executeSQLCommand(0, "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs WHERE iprojectid = " & iprojectid & " AND icardid = " & icardid & " AND icompoundinputid = " & iselectedinputid)

                        End If

                    End If


                    Dim querySumaInsumoCompuesto As String
                    Dim sumaInsumoCompuesto As Double

                    If IsBase = True Then

                        querySumaInsumoCompuesto = "" & _
                        "SELECT SUM(btfci.dcompoundinputqty*bp.dinputfinalprice) AS precio " & _
                        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs btfci " & _
                        "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompoundInput" & iinputid & " ci ON ci.icompoundinputid = btfci.icompoundinputid " & _
                        "LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " i ON i.iinputid = btfci.icompoundinputid " & _
                        "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfci.ibaseid = bp.ibaseid AND btfci.icompoundinputid = bp.iinputid " & _
                        "WHERE btfci.ibaseid = " & iprojectid & " and btfci.icardid = " & icardid & " " & _
                        "GROUP BY ci.iinputid "

                    Else

                        If IsModel = True Then

                            querySumaInsumoCompuesto = "" & _
                            "SELECT SUM(mtfci.dcompoundinputqty*mp.dinputfinalprice) AS precio " & _
                            "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs mtfci " & _
                            "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompoundInput" & iinputid & " ci ON ci.icompoundinputid = mtfci.icompoundinputid " & _
                            "LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " i ON i.iinputid = mtfci.icompoundinputid " & _
                            "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) mp GROUP BY iinputid, imodelid) mp ON mtfci.imodelid = mp.imodelid AND mtfci.icompoundinputid = mp.iinputid " & _
                            "WHERE mtfci.imodelid = " & iprojectid & " and mtfci.icardid = " & icardid & " " & _
                            "GROUP BY ci.iinputid "

                        Else

                            querySumaInsumoCompuesto = "" & _
                            "SELECT SUM(ptfci.dcompoundinputqty*pp.dinputfinalprice) AS precio " & _
                            "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs ptfci " & _
                            "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompoundInput" & iinputid & " ci ON ci.icompoundinputid = ptfci.icompoundinputid " & _
                            "LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " i ON i.iinputid = ptfci.icompoundinputid " & _
                            "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, iprojectid) pp ON ptfci.iprojectid = pp.iprojectid AND ptfci.icompoundinputid = pp.iinputid " & _
                            "WHERE ptfci.iprojectid = " & iprojectid & " and ptfci.icardid = " & icardid & " " & _
                            "GROUP BY ci.iinputid "

                        End If

                    End If

                    sumaInsumoCompuesto = getSQLQueryAsDouble(0, querySumaInsumoCompuesto)

                    txtCostoParaTabulador.Text = FormatCurrency(sumaInsumoCompuesto, 2, TriState.True, TriState.False, TriState.True)

                Else

                    dgvInsumos.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = dselectedinputqty

                End If


            ElseIf dgvInsumos.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = 0 Then

                If MsgBox("¿Estás seguro de que quieres eliminar este Insumo del Insumo Compuesto?", MsgBoxStyle.YesNo, "Confirmación de Eliminación de Insumo de Insumo Compuesto (Err 36)") = MsgBoxResult.Yes Then

                    If IsBase = True Then

                        executeSQLCommand(0, "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs WHERE ibaseid = " & baseid & " AND icardid = " & icardid & " AND icompoundinputid = " & iselectedinputid)

                    Else

                        If IsModel = True Then

                            executeSQLCommand(0, "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs WHERE imodelid = " & iprojectid & " AND icardid = " & icardid & " AND icompoundinputid = " & iselectedinputid)

                        Else

                            executeSQLCommand(0, "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs WHERE iprojectid = " & iprojectid & " AND icardid = " & icardid & " AND icompoundinputid = " & iselectedinputid)

                        End If

                    End If


                    Dim querySumaInsumoCompuesto As String
                    Dim sumaInsumoCompuesto As Double

                    If IsBase = True Then

                        querySumaInsumoCompuesto = "" & _
                        "SELECT SUM(btfci.dcompoundinputqty*bp.dinputfinalprice) AS precio " & _
                        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs btfci " & _
                        "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompoundInput" & iinputid & " ci ON ci.icompoundinputid = btfci.icompoundinputid " & _
                        "LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " i ON i.iinputid = btfci.icompoundinputid " & _
                        "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfci.ibaseid = bp.ibaseid AND btfci.icompoundinputid = bp.iinputid " & _
                        "WHERE btfci.ibaseid = " & iprojectid & " and btfci.icardid = " & icardid & " " & _
                        "GROUP BY ci.iinputid "

                    Else

                        If IsModel = True Then

                            querySumaInsumoCompuesto = "" & _
                            "SELECT SUM(mtfci.dcompoundinputqty*mp.dinputfinalprice) AS precio " & _
                            "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs mtfci " & _
                            "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompoundInput" & iinputid & " ci ON ci.icompoundinputid = mtfci.icompoundinputid " & _
                            "LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " i ON i.iinputid = mtfci.icompoundinputid " & _
                            "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) mp GROUP BY iinputid, imodelid) mp ON mtfci.imodelid = mp.imodelid AND mtfci.icompoundinputid = mp.iinputid " & _
                            "WHERE mtfci.imodelid = " & iprojectid & " and mtfci.icardid = " & icardid & " " & _
                            "GROUP BY ci.iinputid "

                        Else

                            querySumaInsumoCompuesto = "" & _
                            "SELECT SUM(ptfci.dcompoundinputqty*pp.dinputfinalprice) AS precio " & _
                            "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs ptfci " & _
                            "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompoundInput" & iinputid & " ci ON ci.icompoundinputid = ptfci.icompoundinputid " & _
                            "LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " i ON i.iinputid = ptfci.icompoundinputid " & _
                            "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, iprojectid) pp ON ptfci.iprojectid = pp.iprojectid AND ptfci.icompoundinputid = pp.iinputid " & _
                            "WHERE ptfci.iprojectid = " & iprojectid & " and ptfci.icardid = " & icardid & " " & _
                            "GROUP BY ci.iinputid "

                        End If

                    End If

                    sumaInsumoCompuesto = getSQLQueryAsDouble(0, querySumaInsumoCompuesto)

                    txtCostoParaTabulador.Text = FormatCurrency(sumaInsumoCompuesto, 2, TriState.True, TriState.False, TriState.True)

                Else

                    dgvInsumos.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = dselectedinputqty

                End If

            Else

                'Si pone un número

                Dim cantidaddeinsumo As Double = 1.0

                Try
                    cantidaddeinsumo = CDbl(dgvInsumos.Rows(e.RowIndex).Cells(4).Value)
                Catch ex As Exception
                    cantidaddeinsumo = dselectedinputqty
                End Try


                If IsBase = True Then

                    executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs SET dcompoundinputqty = " & dgvInsumos.Rows(e.RowIndex).Cells(e.ColumnIndex).Value & ", iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE ibaseid = " & baseid & " AND icardid = " & icardid & " AND icompoundinputid = " & iselectedinputid)

                Else

                    If IsModel = True Then

                        executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs SET dcompoundinputqty = " & dgvInsumos.Rows(e.RowIndex).Cells(e.ColumnIndex).Value & ", iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE imodelid = " & iprojectid & " AND icardid = " & icardid & " AND icompoundinputid = " & iselectedinputid)

                    Else

                        executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs SET dcompoundinputqty = " & dgvInsumos.Rows(e.RowIndex).Cells(e.ColumnIndex).Value & ", iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE iprojectid = " & iprojectid & " AND icardid = " & icardid & " AND icompoundinputid = " & iselectedinputid)

                    End If

                End If


                Dim querySumaInsumoCompuesto As String
                Dim sumaInsumoCompuesto As Double

                If IsBase = True Then

                    querySumaInsumoCompuesto = "" & _
                    "SELECT SUM(btfci.dcompoundinputqty*bp.dinputfinalprice) AS precio " & _
                    "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs btfci " & _
                    "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompoundInput" & iinputid & " ci ON ci.icompoundinputid = btfci.icompoundinputid " & _
                    "LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " i ON i.iinputid = btfci.icompoundinputid " & _
                    "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfci.ibaseid = bp.ibaseid AND btfci.icompoundinputid = bp.iinputid " & _
                    "WHERE btfci.ibaseid = " & iprojectid & " and btfci.icardid = " & icardid & " " & _
                    "GROUP BY ci.iinputid "

                Else

                    If IsModel = True Then

                        querySumaInsumoCompuesto = "" & _
                        "SELECT SUM(mtfci.dcompoundinputqty*mp.dinputfinalprice) AS precio " & _
                        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs mtfci " & _
                        "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompoundInput" & iinputid & " ci ON ci.icompoundinputid = mtfci.icompoundinputid " & _
                        "LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " i ON i.iinputid = mtfci.icompoundinputid " & _
                        "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) mp GROUP BY iinputid, imodelid) mp ON mtfci.imodelid = mp.imodelid AND mtfci.icompoundinputid = mp.iinputid " & _
                        "WHERE mtfci.imodelid = " & iprojectid & " and mtfci.icardid = " & icardid & " " & _
                        "GROUP BY ci.iinputid "

                    Else

                        querySumaInsumoCompuesto = "" & _
                        "SELECT SUM(ptfci.dcompoundinputqty*pp.dinputfinalprice) AS precio " & _
                        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs ptfci " & _
                        "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompoundInput" & iinputid & " ci ON ci.icompoundinputid = ptfci.icompoundinputid " & _
                        "LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " i ON i.iinputid = ptfci.icompoundinputid " & _
                        "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, iprojectid) pp ON ptfci.iprojectid = pp.iprojectid AND ptfci.icompoundinputid = pp.iinputid " & _
                        "WHERE ptfci.iprojectid = " & iprojectid & " and ptfci.icardid = " & icardid & " " & _
                        "GROUP BY ci.iinputid "

                    End If

                End If

                sumaInsumoCompuesto = getSQLQueryAsDouble(0, querySumaInsumoCompuesto)

                txtCostoParaTabulador.Text = FormatCurrency(sumaInsumoCompuesto, 2, TriState.True, TriState.False, TriState.True)

            End If

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub dgvInsumos_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgvInsumos.EditingControlShowing

        If dgvInsumos.CurrentCell.ColumnIndex = 2 Then

            txtNombreDgvInsumos = CType(e.Control, TextBox)
            txtNombreDgvInsumos_OldText = txtNombreDgvInsumos.Text

        ElseIf dgvInsumos.CurrentCell.ColumnIndex = 4 Then

            txtCantidadDgvInsumos = CType(e.Control, TextBox)
            txtCantidadDgvInsumos_OldText = txtCantidadDgvInsumos.Text

        Else

            txtNombreDgvInsumos = Nothing
            txtNombreDgvInsumos_OldText = Nothing
            txtCantidadDgvInsumos = Nothing
            txtCantidadDgvInsumos_OldText = Nothing

        End If

    End Sub


    Private Sub dgvInsumos_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgvInsumos.KeyUp

        If e.KeyCode = Keys.Delete Then

            If MsgBox("¿Está seguro que deseas eliminar este Insumo del Insumo Compuesto?", MsgBoxStyle.YesNo, "Confirmar Eliminación de Insumo de Insumo Compuesto (Err 39)") = MsgBoxResult.Yes Then

                Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

                Dim tmpselectedinputid As Integer = 0
                Try
                    tmpselectedinputid = CInt(dgvInsumos.CurrentRow.Cells(0).Value)
                Catch ex As Exception

                End Try


                Dim baseid As Integer = 0
                baseid = getSQLQueryAsInteger(0, "SELECT ibaseid FROM base ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

                If baseid = 0 Then
                    baseid = 1
                End If

                If IsBase = True Then

                    executeSQLCommand(0, "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs WHERE ibaseid = " & baseid & " AND icardid = " & icardid & " AND icompoundinputid = " & tmpselectedinputid)

                Else

                    If IsModel = True Then

                        executeSQLCommand(0, "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs WHERE imodelid = " & iprojectid & " AND icardid = " & icardid & " AND icompoundinputid = " & tmpselectedinputid)

                    Else

                        executeSQLCommand(0, "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs WHERE iprojectid = " & iprojectid & " AND icardid = " & icardid & " AND icompoundinputid = " & tmpselectedinputid)

                    End If

                End If


                Dim querySumaInsumoCompuesto As String
                Dim sumaInsumoCompuesto As Double

                If IsBase = True Then

                    querySumaInsumoCompuesto = "" & _
                    "SELECT SUM(btfci.dcompoundinputqty*bp.dinputfinalprice) AS precio " & _
                    "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs btfci " & _
                    "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompoundInput" & iinputid & " ci ON ci.icompoundinputid = btfci.icompoundinputid " & _
                    "LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " i ON i.iinputid = btfci.icompoundinputid " & _
                    "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfci.ibaseid = bp.ibaseid AND btfci.icompoundinputid = bp.iinputid " & _
                    "WHERE btfci.ibaseid = " & iprojectid & " and btfci.icardid = " & icardid & " " & _
                    "GROUP BY ci.iinputid "

                Else

                    If IsModel = True Then

                        querySumaInsumoCompuesto = "" & _
                        "SELECT SUM(mtfci.dcompoundinputqty*mp.dinputfinalprice) AS precio " & _
                        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs mtfci " & _
                        "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompoundInput" & iinputid & " ci ON ci.icompoundinputid = mtfci.icompoundinputid " & _
                        "LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " i ON i.iinputid = mtfci.icompoundinputid " & _
                        "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) mp GROUP BY iinputid, imodelid) mp ON mtfci.imodelid = mp.imodelid AND mtfci.icompoundinputid = mp.iinputid " & _
                        "WHERE mtfci.imodelid = " & iprojectid & " and mtfci.icardid = " & icardid & " " & _
                        "GROUP BY ci.iinputid "

                    Else

                        querySumaInsumoCompuesto = "" & _
                        "SELECT SUM(ptfci.dcompoundinputqty*pp.dinputfinalprice) AS precio " & _
                        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs ptfci " & _
                        "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompoundInput" & iinputid & " ci ON ci.icompoundinputid = ptfci.icompoundinputid " & _
                        "LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " i ON i.iinputid = ptfci.icompoundinputid " & _
                        "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, iprojectid) pp ON ptfci.iprojectid = pp.iprojectid AND ptfci.icompoundinputid = pp.iinputid " & _
                        "WHERE ptfci.iprojectid = " & iprojectid & " and ptfci.icardid = " & icardid & " " & _
                        "GROUP BY ci.iinputid "

                    End If

                End If

                sumaInsumoCompuesto = getSQLQueryAsDouble(0, querySumaInsumoCompuesto)

                txtCostoParaTabulador.Text = FormatCurrency(sumaInsumoCompuesto, 2, TriState.True, TriState.False, TriState.True)


                Dim queryInsumos As String

                If IsBase = True Then

                    queryInsumos = "" & _
                    "SELECT btfci.icompoundinputid, i.sinputdescription AS 'Insumo', btfci.scompoundinputunit AS 'Unidad', btfci.dcompoundinputqty AS 'Cantidad', " & _
                    "FORMAT((btfci.dcompoundinputqty*bp.dinputfinalprice), 3) AS 'Precio' " & _
                    "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs btfci " & _
                    "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompoundInput" & iinputid & " ci ON ci.icompoundinputid = btfci.icompoundinputid " & _
                    "LEFT JOIN inputs i ON i.iinputid = btfci.icompoundinputid " & _
                    "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfci.ibaseid = bp.ibaseid AND btfci.icompoundinputid = bp.iinputid " & _
                    "WHERE btfci.ibaseid = " & iprojectid & " AND btfci.icardid = " & icardid & " AND ci.iinputid = " & iinputid

                Else

                    If IsModel = True Then

                        queryInsumos = "" & _
                        "SELECT mtfci.icompoundinputid, i.sinputdescription AS 'Insumo', mtfci.scompoundinputunit AS 'Unidad', mtfci.dcompoundinputqty AS 'Cantidad', " & _
                        "FORMAT((mtfci.dcompoundinputqty*mp.dinputfinalprice), 3) AS 'Precio' " & _
                        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs mtfci " & _
                        "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompoundInput" & iinputid & " ci ON ci.icompoundinputid = mtfci.icompoundinputid " & _
                        "LEFT JOIN inputs i ON i.iinputid = mtfci.icompoundinputid " & _
                        "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) mp GROUP BY iinputid, imodelid) mp ON mtfci.imodelid = mp.imodelid AND mtfci.icompoundinputid = mp.iinputid " & _
                        "WHERE mtfci.imodelid = " & iprojectid & " AND mtfci.icardid = " & icardid & " AND ci.iinputid = " & iinputid

                    Else

                        queryInsumos = "" & _
                        "SELECT ptfci.icompoundinputid, i.sinputdescription AS 'Insumo', ptfci.scompoundinputunit AS 'Unidad', FORMAT(ptfci.dcompoundinputqty, 3) AS 'Cantidad', " & _
                        "FORMAT((ptfci.dcompoundinputqty*pp.dinputfinalprice), 3) AS 'Precio' " & _
                        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs ptfci " & _
                        "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompoundInput" & iinputid & " ci ON ci.icompoundinputid = ptfci.icompoundinputid " & _
                        "LEFT JOIN inputs i ON i.iinputid = ptfci.icompoundinputid " & _
                        "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, iprojectid) pp ON ptfci.iprojectid = pp.iprojectid AND ptfci.icompoundinputid = pp.iinputid " & _
                        "WHERE ptfci.iprojectid = " & iprojectid & " AND ptfci.icardid = " & icardid & " AND ci.iinputid = " & iinputid

                    End If

                End If

                setDataGridView(dgvInsumos, queryInsumos, False)

                dgvInsumos.Columns(0).Visible = False

                dgvInsumos.Columns(0).ReadOnly = True
                dgvInsumos.Columns(2).ReadOnly = True
                dgvInsumos.Columns(4).ReadOnly = True

                Cursor.Current = System.Windows.Forms.Cursors.Default

            End If

        End If

    End Sub


    Private Sub dgvInsumos_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgvInsumos.Click

        If validaInsumo(True) = True And iinputid <> 0 Then

            dgvInsumos.Enabled = True

        ElseIf validaInsumo(True) = True And iinputid = 0 Then


            'Inicia Save de Master del Insumo Compuesto

            If isFormReadyForAction = False Then
                Exit Sub
            End If

            If IsHistoric = True Then
                Exit Sub
            End If

            Dim unitfound As Boolean = False
            Dim strcaracteresprohibidos As String = "|°!#$%&/()=?¡*¨[]_:;,.-{}+´¿'¬^`~@\<> "

            txtUnidadDeMedida.Text = txtUnidadDeMedida.Text.Trim.Trim(strcaracteresprohibidos.ToCharArray)
            txtUnidadDeMedida.Text = txtUnidadDeMedida.Text.ToUpper

            Dim fecha As Integer = 0
            Dim hora As String = "00:00:00"

            fecha = getAppDate()
            hora = getAppTime()

            Dim baseid As Integer = 0
            baseid = getSQLQueryAsInteger(0, "SELECT ibaseid FROM base ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

            If baseid = 0 Then
                baseid = 1
            End If

            unitfound = getSQLQueryAsBoolean(0, "SELECT count(*) FROM transformationunits WHERE soriginunit = '" & txtUnidadDeMedida.Text.Replace("--", "") & "'")

            If unitfound = False Then

                MsgBox("¡No encontré esa unidad de Medida! ¿Podrías seleccionar una de la lista?", MsgBoxStyle.OkOnly, "Unidad de Medida No Encontrada (Err 91)")

                Dim bu As New BuscaUnidades
                bu.susername = susername
                bu.bactive = bactive
                bu.bonline = bonline
                bu.suserfirstname = suserfirstname
                bu.suserlastname = suserlastname
                bu.suseremail = suseremail
                bu.susersession = susersession
                bu.susermachinename = susermachinename
                bu.suserip = suserip
                bu.permisoActivosCrear = permisoActivosCrear
                bu.permisoActivosEliminar = permisoActivosEliminar
                bu.permisoActivosModificar = permisoActivosModificar
                bu.permisoActivosVer = permisoActivosVer
                bu.permisoAnalisisDeUtilidadesCrear = permisoAnalisisDeUtilidadesCrear
                bu.permisoAnalisisDeUtilidadesEliminar = permisoAnalisisDeUtilidadesEliminar
                bu.permisoAnalisisDeUtilidadesModificar = permisoAnalisisDeUtilidadesModificar
                bu.permisoAnalisisDeUtilidadesVer = permisoAnalisisDeUtilidadesVer
                bu.permisoPersonasCrear = permisoPersonasCrear
                bu.permisoPersonasEliminar = permisoPersonasEliminar
                bu.permisoPersonasModificar = permisoPersonasModificar
                bu.permisoPersonasVer = permisoPersonasVer
                bu.permisoCotizacionesCrear = permisoCotizacionesCrear
                bu.permisoCotizacionesEliminar = permisoCotizacionesEliminar
                bu.permisoCotizacionesModificar = permisoCotizacionesModificar
                bu.permisoCotizacionesVer = permisoCotizacionesVer
                bu.permisoEnviosCrear = permisoEnviosCrear
                bu.permisoEnviosEliminar = permisoEnviosEliminar
                bu.permisoEnviosModificar = permisoEnviosModificar
                bu.permisoEnviosVer = permisoEnviosVer
                bu.permisoEquivalenciasCrear = permisoEquivalenciasCrear
                bu.permisoEquivalenciasEliminar = permisoEquivalenciasEliminar
                bu.permisoEquivalenciasModificar = permisoEquivalenciasModificar
                bu.permisoEquivalenciasVer = permisoEquivalenciasVer
                bu.permisoFacturasMRDCrear = permisoFacturasMRDCrear
                bu.permisoFacturasMRDEliminar = permisoFacturasMRDEliminar
                bu.permisoFacturasMRDModificar = permisoFacturasMRDModificar
                bu.permisoFacturasMRDVer = permisoFacturasMRDVer
                bu.permisoGastosAsignarAActivo = permisoGastosAsignarAActivo
                bu.permisoGastosAsignarAProyecto = permisoGastosAsignarAProyecto
                bu.permisoGastosCrear = permisoGastosCrear
                bu.permisoGastosEliminar = permisoGastosEliminar
                bu.permisoGastosModificar = permisoGastosModificar
                bu.permisoGastosVer = permisoGastosVer
                bu.permisoIngresosCrear = permisoIngresosCrear
                bu.permisoIngresosEliminar = permisoIngresosEliminar
                bu.permisoIngresosModificar = permisoIngresosModificar
                bu.permisoIngresosVer = permisoIngresosVer
                bu.permisoInsumosCrear = permisoInsumosCrear
                bu.permisoInsumosEliminar = permisoInsumosEliminar
                bu.permisoInsumosModificar = permisoInsumosModificar
                bu.permisoInsumosVer = permisoInsumosVer
                bu.permisoInventarioCrear = permisoInventarioCrear
                bu.permisoInventarioEliminar = permisoInventarioEliminar
                bu.permisoInventarioModificar = permisoInventarioModificar
                bu.permisoInventarioVer = permisoInventarioVer
                bu.permisoLogsEliminar = permisoLogsEliminar
                bu.permisoLogsVer = permisoLogsVer
                bu.permisoModelosCrear = permisoModelosCrear
                bu.permisoModelosEliminar = permisoModelosEliminar
                bu.permisoModelosModificar = permisoModelosModificar
                bu.permisoModelosPlanos = permisoModelosPlanos
                bu.permisoModelosVer = permisoModelosVer
                bu.permisoOrdenesCrear = permisoOrdenesCrear
                bu.permisoOrdenesEliminar = permisoOrdenesEliminar
                bu.permisoOrdenesModificar = permisoOrdenesModificar
                bu.permisoOrdenesVer = permisoOrdenesVer
                bu.permisoPersonasCrear = permisoPersonasCrear
                bu.permisoPersonasEliminar = permisoPersonasEliminar
                bu.permisoPersonasModificar = permisoPersonasModificar
                bu.permisoPersonasVer = permisoPersonasVer
                bu.permisoProveedoresCrear = permisoProveedoresCrear
                bu.permisoProveedoresEliminar = permisoProveedoresEliminar
                bu.permisoProveedoresModificar = permisoProveedoresModificar
                bu.permisoProveedoresVer = permisoProveedoresVer
                bu.permisoProyectosCrear = permisoProyectosCrear
                bu.permisoProyectosEliminar = permisoProyectosEliminar
                bu.permisoProyectosModificar = permisoProyectosModificar
                bu.permisoProyectosPlanos = permisoProyectosPlanos
                bu.permisoProyectosVer = permisoProyectosVer
                bu.permisoResumenDeTarjetasCrear = permisoResumenDeTarjetasCrear
                bu.permisoResumenDeTarjetasEliminar = permisoResumenDeTarjetasEliminar
                bu.permisoResumenDeTarjetasModificar = permisoResumenDeTarjetasModificar
                bu.permisoResumenDeTarjetasVer = permisoResumenDeTarjetasVer
                bu.permisoValesDeGasolinaCrear = permisoValesDeGasolinaCrear
                bu.permisoValesDeGasolinaEliminar = permisoValesDeGasolinaEliminar
                bu.permisoValesDeGasolinaModificar = permisoValesDeGasolinaModificar
                bu.permisoValesDeGasolinaVer = permisoValesDeGasolinaVer

                bu.querystring = txtUnidadDeMedida.Text

                bu.isEdit = False

                bu.ShowDialog(Me)

                If bu.DialogResult = Windows.Forms.DialogResult.OK Then

                    txtUnidadDeMedida.Text = bu.sunit1

                    If validaInsumo(True) = False Then
                        Cursor.Current = System.Windows.Forms.Cursors.Default
                        Exit Sub
                    Else
                        dgvInsumos.Enabled = True
                    End If

                    If IsEdit = True Then

                        Dim queries(3) As String

                        queries(0) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " SET iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "', sinputdescription = '" & txtNombreDelInsumo.Text & "', sinputunit = '" & txtUnidadDeMedida.Text & "' WHERE iinputid = " & iinputid
                        queries(1) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types SET sinputtypedescription = '" & cmbTipoDeInsumo.SelectedItem.ToString & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE iinputid = " & iinputid

                        If IsBase = True Then
                            queries(2) = "INSERT INTO logs VALUES (" & getAppDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Presupuesto Base " & iprojectid & "', 'OK')"
                        Else
                            If IsModel = True Then
                                queries(2) = "INSERT INTO logs VALUES (" & getAppDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Modelo " & iprojectid & " : " & getSQLQueryAsString(0, "SELECT smodelname FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & " WHERE imodelid = " & iprojectid) & "', 'OK')"
                            Else
                                queries(2) = "INSERT INTO logs VALUES (" & getAppDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Proyecto " & iprojectid & " : " & getSQLQueryAsString(0, "SELECT sprojectname FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & " WHERE iprojectid = " & iprojectid) & "', 'OK')"
                            End If
                        End If

                        executeTransactedSQLCommand(0, queries)

                    Else

                        Dim checkIfItsOnlyTextUpdate As Boolean = False

                        checkIfItsOnlyTextUpdate = getSQLQueryAsBoolean(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " WHERE iinputid = " & iinputid)

                        If checkIfItsOnlyTextUpdate = True Then

                            Dim queries(3) As String

                            queries(0) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " SET iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "', sinputdescription = '" & txtNombreDelInsumo.Text & "', sinputunit = '" & txtUnidadDeMedida.Text & "' WHERE iinputid = " & iinputid

                            If iinputid <> 0 Then
                                queries(1) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types SET sinputtypedescription = '" & cmbTipoDeInsumo.SelectedItem.ToString & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE iinputid = " & iinputid
                            End If

                            If IsBase = True Then
                                queries(2) = "INSERT INTO logs VALUES (" & getAppDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Presupuesto Base " & iprojectid & "', 'OK')"
                            Else
                                If IsModel = True Then
                                    queries(2) = "INSERT INTO logs VALUES (" & getAppDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Modelo " & iprojectid & " : " & getSQLQueryAsString(0, "SELECT smodelname FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & " WHERE imodelid = " & iprojectid) & "', 'OK')"
                                Else
                                    queries(2) = "INSERT INTO logs VALUES (" & getAppDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Proyecto " & iprojectid & " : " & getSQLQueryAsString(0, "SELECT sprojectname FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & " WHERE iprojectid = " & iprojectid) & "', 'OK')"
                                End If
                            End If

                            executeTransactedSQLCommand(0, queries)

                        Else

                            Dim queries(3) As String

                            iinputid = getSQLQueryAsInteger(0, "SELECT IF(MAX(iinputid) + 1 IS NULL, 1, MAX(iinputid) + 1) AS iinputid FROM inputs ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

                            Dim queriesCreation(9) As String

                            queriesCreation(0) = "DROP TABLE IF EXISTS oversight. tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input0"
                            queriesCreation(1) = "DROP TABLE IF EXISTS oversight. tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid
                            queriesCreation(2) = "CREATE TABLE  oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " ( `iinputid` int(11) NOT NULL AUTO_INCREMENT, `sinputdescription` varchar(300) CHARACTER SET latin1 NOT NULL, `sinputunit` varchar(100) CHARACTER SET latin1 NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL,  PRIMARY KEY (`iinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

                            queriesCreation(3) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompoundInput0"
                            queriesCreation(4) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompoundInput" & iinputid
                            queriesCreation(5) = "CREATE TABLE  oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompoundInput" & iinputid & " ( `iinputid` int(11) NOT NULL, `icompoundinputid` int(11) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`iinputid`,`icompoundinputid`) USING BTREE, KEY `inputid` (`iinputid`), KEY `compoundinputid` (`icompoundinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

                            queriesCreation(6) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input0Types"
                            queriesCreation(7) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types"
                            queriesCreation(8) = "CREATE TABLE  oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types" & " ( `iinputid` int(11) NOT NULL, `sinputtypedescription` varchar(250) CHARACTER SET latin1 NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`iinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci COMMENT='Only to differ which input is taken as Mano de Obra'"

                            executeTransactedSQLCommand(0, queriesCreation)

                            queries(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " VALUES (" & iinputid & ", '" & txtNombreDelInsumo.Text & "', '" & txtUnidadDeMedida.Text & "', " & fecha & ", '" & hora & "', '" & susername & "')"

                            queries(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types VALUES (" & iinputid & ", '" & cmbTipoDeInsumo.SelectedItem.ToString & "', " & fecha & ", '" & hora & "', '" & susername & "')"

                            If IsBase = True Then
                                queries(2) = "INSERT INTO logs VALUES (" & getAppDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Presupuesto Base " & iprojectid & "', 'OK')"
                            Else
                                If IsModel = True Then
                                    queries(2) = "INSERT INTO logs VALUES (" & getAppDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Modelo " & iprojectid & " : " & getSQLQueryAsString(0, "SELECT smodelname FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & " WHERE imodelid = " & iprojectid) & "', 'OK')"
                                Else
                                    queries(2) = "INSERT INTO logs VALUES (" & getAppDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Proyecto " & iprojectid & " : " & getSQLQueryAsString(0, "SELECT sprojectname FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & " WHERE iprojectid = " & iprojectid) & "', 'OK')"
                                End If
                            End If

                            executeTransactedSQLCommand(0, queries)

                        End If

                    End If

                Else

                    txtUnidadDeMedida.Focus()
                    Exit Sub

                End If

            Else


                If validaInsumo(True) = False Then
                    Cursor.Current = System.Windows.Forms.Cursors.Default
                    Exit Sub
                Else
                    dgvInsumos.Enabled = True
                End If

                If IsEdit = True Then

                    Dim queries(3) As String

                    queries(0) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " SET iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "', sinputdescription = '" & txtNombreDelInsumo.Text & "', sinputunit = '" & txtUnidadDeMedida.Text & "' WHERE iinputid = " & iinputid
                    queries(1) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types SET sinputtypedescription = '" & cmbTipoDeInsumo.SelectedItem.ToString & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE iinputid = " & iinputid

                    If IsBase = True Then
                        queries(2) = "INSERT INTO logs VALUES (" & getAppDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Presupuesto Base " & iprojectid & "', 'OK')"
                    Else
                        If IsModel = True Then
                            queries(2) = "INSERT INTO logs VALUES (" & getAppDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Modelo " & iprojectid & " : " & getSQLQueryAsString(0, "SELECT smodelname FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & " WHERE imodelid = " & iprojectid) & "', 'OK')"
                        Else
                            queries(2) = "INSERT INTO logs VALUES (" & getAppDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Proyecto " & iprojectid & " : " & getSQLQueryAsString(0, "SELECT sprojectname FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & " WHERE iprojectid = " & iprojectid) & "', 'OK')"
                        End If
                    End If

                    executeTransactedSQLCommand(0, queries)

                Else

                    Dim checkIfItsOnlyTextUpdate As Boolean = False

                    checkIfItsOnlyTextUpdate = getSQLQueryAsBoolean(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " WHERE iinputid = " & iinputid)

                    If checkIfItsOnlyTextUpdate = True Then

                        Dim queries(3) As String

                        queries(0) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " SET iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "', sinputdescription = '" & txtNombreDelInsumo.Text & "', sinputunit = '" & txtUnidadDeMedida.Text & "' WHERE iinputid = " & iinputid

                        If iinputid <> 0 Then
                            queries(1) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types SET sinputtypedescription = '" & cmbTipoDeInsumo.SelectedItem.ToString & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE iinputid = " & iinputid
                        End If

                        If IsBase = True Then
                            queries(2) = "INSERT INTO logs VALUES (" & getAppDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Presupuesto Base " & iprojectid & "', 'OK')"
                        Else
                            If IsModel = True Then
                                queries(2) = "INSERT INTO logs VALUES (" & getAppDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Modelo " & iprojectid & " : " & getSQLQueryAsString(0, "SELECT smodelname FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & " WHERE imodelid = " & iprojectid) & "', 'OK')"
                            Else
                                queries(2) = "INSERT INTO logs VALUES (" & getAppDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Proyecto " & iprojectid & " : " & getSQLQueryAsString(0, "SELECT sprojectname FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & " WHERE iprojectid = " & iprojectid) & "', 'OK')"
                            End If
                        End If

                        executeTransactedSQLCommand(0, queries)

                    Else

                        Dim queries(3) As String

                        iinputid = getSQLQueryAsInteger(0, "SELECT IF(MAX(iinputid) + 1 IS NULL, 1, MAX(iinputid) + 1) AS iinputid FROM inputs ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

                        Dim queriesCreation(9) As String

                        queriesCreation(0) = "DROP TABLE IF EXISTS oversight. tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input0"
                        queriesCreation(1) = "DROP TABLE IF EXISTS oversight. tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid
                        queriesCreation(2) = "CREATE TABLE  oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " ( `iinputid` int(11) NOT NULL AUTO_INCREMENT, `sinputdescription` varchar(300) CHARACTER SET latin1 NOT NULL, `sinputunit` varchar(100) CHARACTER SET latin1 NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL,  PRIMARY KEY (`iinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

                        queriesCreation(3) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompoundInput0"
                        queriesCreation(4) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompoundInput" & iinputid
                        queriesCreation(5) = "CREATE TABLE  oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompoundInput" & iinputid & " ( `iinputid` int(11) NOT NULL, `icompoundinputid` int(11) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`iinputid`,`icompoundinputid`) USING BTREE, KEY `inputid` (`iinputid`), KEY `compoundinputid` (`icompoundinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

                        queriesCreation(6) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input0Types"
                        queriesCreation(7) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types"
                        queriesCreation(8) = "CREATE TABLE  oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types" & " ( `iinputid` int(11) NOT NULL, `sinputtypedescription` varchar(250) CHARACTER SET latin1 NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`iinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci COMMENT='Only to differ which input is taken as Mano de Obra'"

                        executeTransactedSQLCommand(0, queriesCreation)

                        queries(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " VALUES (" & iinputid & ", '" & txtNombreDelInsumo.Text & "', '" & txtUnidadDeMedida.Text & "', " & fecha & ", '" & hora & "', '" & susername & "')"

                        queries(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types VALUES (" & iinputid & ", '" & cmbTipoDeInsumo.SelectedItem.ToString & "', " & fecha & ", '" & hora & "', '" & susername & "')"

                        If IsBase = True Then
                            queries(2) = "INSERT INTO logs VALUES (" & getAppDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Presupuesto Base " & iprojectid & "', 'OK')"
                        Else
                            If IsModel = True Then
                                queries(2) = "INSERT INTO logs VALUES (" & getAppDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Modelo " & iprojectid & " : " & getSQLQueryAsString(0, "SELECT smodelname FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & " WHERE imodelid = " & iprojectid) & "', 'OK')"
                            Else
                                queries(2) = "INSERT INTO logs VALUES (" & getAppDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Proyecto " & iprojectid & " : " & getSQLQueryAsString(0, "SELECT sprojectname FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & " WHERE iprojectid = " & iprojectid) & "', 'OK')"
                            End If
                        End If

                        executeTransactedSQLCommand(0, queries)

                    End If

                End If


            End If


            'Activamos el grid


            dgvInsumos.Enabled = True

        End If

    End Sub


    Private Sub dgvInsumos_UserAddedRow(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowEventArgs) Handles dgvInsumos.UserAddedRow

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        isFormReadyForAction = False

        Dim bi As New BuscaInsumos
        bi.susername = susername
        bi.bactive = bactive
        bi.bonline = bonline
        bi.suserfirstname = suserfirstname
        bi.suserlastname = suserlastname
        bi.suseremail = suseremail
        bi.susersession = susersession
        bi.susermachinename = susermachinename
        bi.suserip = suserip
        bi.permisoActivosCrear = permisoActivosCrear
        bi.permisoActivosEliminar = permisoActivosEliminar
        bi.permisoActivosModificar = permisoActivosModificar
        bi.permisoActivosVer = permisoActivosVer
        bi.permisoAnalisisDeUtilidadesCrear = permisoAnalisisDeUtilidadesCrear
        bi.permisoAnalisisDeUtilidadesEliminar = permisoAnalisisDeUtilidadesEliminar
        bi.permisoAnalisisDeUtilidadesModificar = permisoAnalisisDeUtilidadesModificar
        bi.permisoAnalisisDeUtilidadesVer = permisoAnalisisDeUtilidadesVer
        bi.permisoPersonasCrear = permisoPersonasCrear
        bi.permisoPersonasEliminar = permisoPersonasEliminar
        bi.permisoPersonasModificar = permisoPersonasModificar
        bi.permisoPersonasVer = permisoPersonasVer
        bi.permisoCotizacionesCrear = permisoCotizacionesCrear
        bi.permisoCotizacionesEliminar = permisoCotizacionesEliminar
        bi.permisoCotizacionesModificar = permisoCotizacionesModificar
        bi.permisoCotizacionesVer = permisoCotizacionesVer
        bi.permisoEnviosCrear = permisoEnviosCrear
        bi.permisoEnviosEliminar = permisoEnviosEliminar
        bi.permisoEnviosModificar = permisoEnviosModificar
        bi.permisoEnviosVer = permisoEnviosVer
        bi.permisoEquivalenciasCrear = permisoEquivalenciasCrear
        bi.permisoEquivalenciasEliminar = permisoEquivalenciasEliminar
        bi.permisoEquivalenciasModificar = permisoEquivalenciasModificar
        bi.permisoEquivalenciasVer = permisoEquivalenciasVer
        bi.permisoFacturasMRDCrear = permisoFacturasMRDCrear
        bi.permisoFacturasMRDEliminar = permisoFacturasMRDEliminar
        bi.permisoFacturasMRDModificar = permisoFacturasMRDModificar
        bi.permisoFacturasMRDVer = permisoFacturasMRDVer
        bi.permisoGastosAsignarAActivo = permisoGastosAsignarAActivo
        bi.permisoGastosAsignarAProyecto = permisoGastosAsignarAProyecto
        bi.permisoGastosCrear = permisoGastosCrear
        bi.permisoGastosEliminar = permisoGastosEliminar
        bi.permisoGastosModificar = permisoGastosModificar
        bi.permisoGastosVer = permisoGastosVer
        bi.permisoIngresosCrear = permisoIngresosCrear
        bi.permisoIngresosEliminar = permisoIngresosEliminar
        bi.permisoIngresosModificar = permisoIngresosModificar
        bi.permisoIngresosVer = permisoIngresosVer
        bi.permisoInsumosCrear = permisoInsumosCrear
        bi.permisoInsumosEliminar = permisoInsumosEliminar
        bi.permisoInsumosModificar = permisoInsumosModificar
        bi.permisoInsumosVer = permisoInsumosVer
        bi.permisoInventarioCrear = permisoInventarioCrear
        bi.permisoInventarioEliminar = permisoInventarioEliminar
        bi.permisoInventarioModificar = permisoInventarioModificar
        bi.permisoInventarioVer = permisoInventarioVer
        bi.permisoLogsEliminar = permisoLogsEliminar
        bi.permisoLogsVer = permisoLogsVer
        bi.permisoModelosCrear = permisoModelosCrear
        bi.permisoModelosEliminar = permisoModelosEliminar
        bi.permisoModelosModificar = permisoModelosModificar
        bi.permisoModelosPlanos = permisoModelosPlanos
        bi.permisoModelosVer = permisoModelosVer
        bi.permisoOrdenesCrear = permisoOrdenesCrear
        bi.permisoOrdenesEliminar = permisoOrdenesEliminar
        bi.permisoOrdenesModificar = permisoOrdenesModificar
        bi.permisoOrdenesVer = permisoOrdenesVer
        bi.permisoPersonasCrear = permisoPersonasCrear
        bi.permisoPersonasEliminar = permisoPersonasEliminar
        bi.permisoPersonasModificar = permisoPersonasModificar
        bi.permisoPersonasVer = permisoPersonasVer
        bi.permisoProveedoresCrear = permisoProveedoresCrear
        bi.permisoProveedoresEliminar = permisoProveedoresEliminar
        bi.permisoProveedoresModificar = permisoProveedoresModificar
        bi.permisoProveedoresVer = permisoProveedoresVer
        bi.permisoProyectosCrear = permisoProyectosCrear
        bi.permisoProyectosEliminar = permisoProyectosEliminar
        bi.permisoProyectosModificar = permisoProyectosModificar
        bi.permisoProyectosPlanos = permisoProyectosPlanos
        bi.permisoProyectosVer = permisoProyectosVer
        bi.permisoResumenDeTarjetasCrear = permisoResumenDeTarjetasCrear
        bi.permisoResumenDeTarjetasEliminar = permisoResumenDeTarjetasEliminar
        bi.permisoResumenDeTarjetasModificar = permisoResumenDeTarjetasModificar
        bi.permisoResumenDeTarjetasVer = permisoResumenDeTarjetasVer
        bi.permisoValesDeGasolinaCrear = permisoValesDeGasolinaCrear
        bi.permisoValesDeGasolinaEliminar = permisoValesDeGasolinaEliminar
        bi.permisoValesDeGasolinaModificar = permisoValesDeGasolinaModificar
        bi.permisoValesDeGasolinaVer = permisoValesDeGasolinaVer

        bi.querystring = dgvInsumos.CurrentCell.EditedFormattedValue

        bi.IsEdit = False

        bi.IsBase = IsBase
        bi.IsModel = IsModel

        bi.ShowDialog(Me)

        If bi.DialogResult = Windows.Forms.DialogResult.OK Then

            Dim baseid As Integer = 0
            baseid = getSQLQueryAsInteger(0, "SELECT ibaseid FROM base ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

            If baseid = 0 Then
                baseid = 1
            End If

            Dim fecha As Integer = 0
            Dim hora As String = ""

            fecha = getAppDate()
            hora = getAppTime()

            Dim dsBusquedaInsumosRepetidos As DataSet

            If IsBase = True Then

                dsBusquedaInsumosRepetidos = getSQLQueryAsDataset(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs WHERE ibaseid = " & baseid & " AND icardid = " & icardid & " AND icompoundinputid = " & bi.iinputid)

            Else

                If IsModel = True Then

                    dsBusquedaInsumosRepetidos = getSQLQueryAsDataset(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs WHERE imodelid = " & iprojectid & " AND icardid = " & icardid & " AND icompoundinputid = " & bi.iinputid)

                Else

                    dsBusquedaInsumosRepetidos = getSQLQueryAsDataset(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs WHERE iprojectid = " & iprojectid & " AND icardid = " & icardid & " AND icompoundinputid = " & bi.iinputid)

                End If

            End If


            If dsBusquedaInsumosRepetidos.Tables(0).Rows.Count > 0 Then

                MsgBox("Ya tienes ese Insumo insertado en este Insumo Compuesto. ¿Podrías buscarlo en la lista y cambiar la cantidad si así lo deseas?", MsgBoxStyle.OkOnly, "Insumo Repetido (Err 83)")
                dgvInsumos.EndEdit()
                Exit Sub

            End If

            Dim chequeoPrimeraVezQueSeInsertaAlgo As Integer = 0

            If IsBase = True And baseid <> iprojectid Then

                chequeoPrimeraVezQueSeInsertaAlgo = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices WHERE ibaseid = " & iprojectid)

                If chequeoPrimeraVezQueSeInsertaAlgo = 0 Then
                    executeSQLCommand(0, "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices SELECT " & iprojectid & ", iinputid, dinputpricewithoutIVA, dinputprotectionpercentage, dinputfinalprice, " & fecha & ", '" & hora & "', '" & susername & "' FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices WHERE ibaseid = " & baseid & " ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid")
                End If

            ElseIf IsModel = True Then

                chequeoPrimeraVezQueSeInsertaAlgo = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Prices WHERE imodelid = " & iprojectid)

                If chequeoPrimeraVezQueSeInsertaAlgo = 0 Then
                    executeSQLCommand(0, "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Prices SELECT " & iprojectid & ", iinputid, dinputpricewithoutIVA, dinputprotectionpercentage, dinputfinalprice, " & fecha & ", '" & hora & "', '" & susername & "' FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices WHERE ibaseid = " & baseid & " ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid")
                End If

            ElseIf IsModel = False And IsBase = False Then

                chequeoPrimeraVezQueSeInsertaAlgo = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Prices WHERE iprojectid = " & iprojectid)

                If chequeoPrimeraVezQueSeInsertaAlgo = 0 Then
                    executeSQLCommand(0, "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Prices SELECT " & iprojectid & ", iinputid, dinputpricewithoutIVA, dinputprotectionpercentage, dinputfinalprice, " & fecha & ", '" & hora & "', '" & susername & "' FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices WHERE ibaseid = " & baseid & " ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid")
                End If

            End If


            If IsBase = True Then

                Dim queries(2) As String

                queries(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompoundInput" & iinputid & " VALUES (" & iinputid & ", " & bi.iinputid & ", " & fecha & ", '" & hora & "', '" & susername & "')"
                queries(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs SELECT " & baseid & ", " & icardid & ", " & bi.iinputid & ", sinputunit, 1, " & fecha & ", '" & hora & "', '" & susername & "' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " WHERE iinputid = " & iinputid

                executeTransactedSQLCommand(0, queries)

            Else

                If IsModel = True Then

                    Dim queries(2) As String

                    queries(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompoundInput" & iinputid & " VALUES (" & iinputid & ", " & bi.iinputid & ", " & fecha & ", '" & hora & "', '" & susername & "')"
                    queries(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs SELECT " & iprojectid & ", " & icardid & ", " & bi.iinputid & ", sinputunit, 1, " & fecha & ", '" & hora & "', '" & susername & "' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " WHERE iinputid = " & iinputid

                    executeTransactedSQLCommand(0, queries)

                Else

                    Dim queries(2) As String

                    queries(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompoundInput" & iinputid & " VALUES (" & iinputid & ", " & bi.iinputid & ", " & fecha & ", '" & hora & "', '" & susername & "')"
                    queries(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs SELECT " & iprojectid & ", " & icardid & ", " & bi.iinputid & ", sinputunit, 1, " & fecha & ", '" & hora & "', '" & susername & "' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " WHERE iinputid = " & iinputid

                    executeTransactedSQLCommand(0, queries)

                End If

            End If

            Dim querySumaInsumoCompuesto As String
            Dim sumaInsumoCompuesto As Double

            If IsBase = True Then

                querySumaInsumoCompuesto = "" & _
                "SELECT SUM(btfci.dcompoundinputqty*bp.dinputfinalprice) AS precio " & _
                "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs btfci " & _
                "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompoundInput" & iinputid & " ci ON ci.icompoundinputid = btfci.icompoundinputid " & _
                "LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " i ON i.iinputid = btfci.icompoundinputid " & _
                "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfci.ibaseid = bp.ibaseid AND btfci.icompoundinputid = bp.iinputid " & _
                "WHERE btfci.ibaseid = " & iprojectid & " and btfci.icardid = " & icardid & " " & _
                "GROUP BY ci.iinputid "

            Else

                If IsModel = True Then

                    querySumaInsumoCompuesto = "" & _
                    "SELECT SUM(mtfci.dcompoundinputqty*mp.dinputfinalprice) AS precio " & _
                    "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs mtfci " & _
                    "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompoundInput" & iinputid & " ci ON ci.icompoundinputid = mtfci.icompoundinputid " & _
                    "LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " i ON i.iinputid = mtfci.icompoundinputid " & _
                    "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) mp GROUP BY iinputid, imodelid) mp ON mtfci.imodelid = mp.imodelid AND mtfci.icompoundinputid = mp.iinputid " & _
                    "WHERE mtfci.imodelid = " & iprojectid & " and mtfci.icardid = " & icardid & " " & _
                    "GROUP BY ci.iinputid "

                Else

                    querySumaInsumoCompuesto = "" & _
                    "SELECT SUM(ptfci.dcompoundinputqty*pp.dinputfinalprice) AS precio " & _
                    "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs ptfci " & _
                    "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompoundInput" & iinputid & " ci ON ci.icompoundinputid = ptfci.icompoundinputid " & _
                    "LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " i ON i.iinputid = ptfci.icompoundinputid " & _
                    "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, iprojectid) pp ON ptfci.iprojectid = pp.iprojectid AND ptfci.icompoundinputid = pp.iinputid " & _
                    "WHERE ptfci.iprojectid = " & iprojectid & " and ptfci.icardid = " & icardid & " " & _
                    "GROUP BY ci.iinputid "

                End If

            End If

            sumaInsumoCompuesto = getSQLQueryAsDouble(0, querySumaInsumoCompuesto)

            txtCostoParaTabulador.Text = FormatCurrency(sumaInsumoCompuesto, 2, TriState.True, TriState.False, TriState.True)

            dgvInsumos.EndEdit()

            isFormReadyForAction = True

        Else

            dgvInsumos.EndEdit()

            isFormReadyForAction = True

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub btnNuevoInsumo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNuevoInsumo.Click


        'Inicia Save de Master del Insumo Compuesto

        If isFormReadyForAction = False Then
            Exit Sub
        End If

        If IsHistoric = True Then
            Exit Sub
        End If

        Dim unitfound As Boolean = False
        Dim strcaracteresprohibidos As String = "|°!#$%&/()=?¡*¨[]_:;,.-{}+´¿'¬^`~@\<> "

        txtUnidadDeMedida.Text = txtUnidadDeMedida.Text.Trim.Trim(strcaracteresprohibidos.ToCharArray)
        txtUnidadDeMedida.Text = txtUnidadDeMedida.Text.ToUpper

        Dim fecha As Integer = 0
        Dim hora As String = "00:00:00"

        fecha = getAppDate()
        hora = getAppTime()

        Dim baseid As Integer = 0
        baseid = getSQLQueryAsInteger(0, "SELECT ibaseid FROM base ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

        If baseid = 0 Then
            baseid = 1
        End If

        unitfound = getSQLQueryAsBoolean(0, "SELECT count(*) FROM transformationunits WHERE soriginunit = '" & txtUnidadDeMedida.Text.Replace("--", "") & "'")

        If unitfound = False Then

            MsgBox("¡No encontré esa unidad de Medida! ¿Podrías seleccionar una de la lista?", MsgBoxStyle.OkOnly, "Unidad de Medida No Encontrada (Err 91)")

            Dim bu As New BuscaUnidades
            bu.susername = susername
            bu.bactive = bactive
            bu.bonline = bonline
            bu.suserfirstname = suserfirstname
            bu.suserlastname = suserlastname
            bu.suseremail = suseremail
            bu.susersession = susersession
            bu.susermachinename = susermachinename
            bu.suserip = suserip
            bu.permisoActivosCrear = permisoActivosCrear
            bu.permisoActivosEliminar = permisoActivosEliminar
            bu.permisoActivosModificar = permisoActivosModificar
            bu.permisoActivosVer = permisoActivosVer
            bu.permisoAnalisisDeUtilidadesCrear = permisoAnalisisDeUtilidadesCrear
            bu.permisoAnalisisDeUtilidadesEliminar = permisoAnalisisDeUtilidadesEliminar
            bu.permisoAnalisisDeUtilidadesModificar = permisoAnalisisDeUtilidadesModificar
            bu.permisoAnalisisDeUtilidadesVer = permisoAnalisisDeUtilidadesVer
            bu.permisoPersonasCrear = permisoPersonasCrear
            bu.permisoPersonasEliminar = permisoPersonasEliminar
            bu.permisoPersonasModificar = permisoPersonasModificar
            bu.permisoPersonasVer = permisoPersonasVer
            bu.permisoCotizacionesCrear = permisoCotizacionesCrear
            bu.permisoCotizacionesEliminar = permisoCotizacionesEliminar
            bu.permisoCotizacionesModificar = permisoCotizacionesModificar
            bu.permisoCotizacionesVer = permisoCotizacionesVer
            bu.permisoEnviosCrear = permisoEnviosCrear
            bu.permisoEnviosEliminar = permisoEnviosEliminar
            bu.permisoEnviosModificar = permisoEnviosModificar
            bu.permisoEnviosVer = permisoEnviosVer
            bu.permisoEquivalenciasCrear = permisoEquivalenciasCrear
            bu.permisoEquivalenciasEliminar = permisoEquivalenciasEliminar
            bu.permisoEquivalenciasModificar = permisoEquivalenciasModificar
            bu.permisoEquivalenciasVer = permisoEquivalenciasVer
            bu.permisoFacturasMRDCrear = permisoFacturasMRDCrear
            bu.permisoFacturasMRDEliminar = permisoFacturasMRDEliminar
            bu.permisoFacturasMRDModificar = permisoFacturasMRDModificar
            bu.permisoFacturasMRDVer = permisoFacturasMRDVer
            bu.permisoGastosAsignarAActivo = permisoGastosAsignarAActivo
            bu.permisoGastosAsignarAProyecto = permisoGastosAsignarAProyecto
            bu.permisoGastosCrear = permisoGastosCrear
            bu.permisoGastosEliminar = permisoGastosEliminar
            bu.permisoGastosModificar = permisoGastosModificar
            bu.permisoGastosVer = permisoGastosVer
            bu.permisoIngresosCrear = permisoIngresosCrear
            bu.permisoIngresosEliminar = permisoIngresosEliminar
            bu.permisoIngresosModificar = permisoIngresosModificar
            bu.permisoIngresosVer = permisoIngresosVer
            bu.permisoInsumosCrear = permisoInsumosCrear
            bu.permisoInsumosEliminar = permisoInsumosEliminar
            bu.permisoInsumosModificar = permisoInsumosModificar
            bu.permisoInsumosVer = permisoInsumosVer
            bu.permisoInventarioCrear = permisoInventarioCrear
            bu.permisoInventarioEliminar = permisoInventarioEliminar
            bu.permisoInventarioModificar = permisoInventarioModificar
            bu.permisoInventarioVer = permisoInventarioVer
            bu.permisoLogsEliminar = permisoLogsEliminar
            bu.permisoLogsVer = permisoLogsVer
            bu.permisoModelosCrear = permisoModelosCrear
            bu.permisoModelosEliminar = permisoModelosEliminar
            bu.permisoModelosModificar = permisoModelosModificar
            bu.permisoModelosPlanos = permisoModelosPlanos
            bu.permisoModelosVer = permisoModelosVer
            bu.permisoOrdenesCrear = permisoOrdenesCrear
            bu.permisoOrdenesEliminar = permisoOrdenesEliminar
            bu.permisoOrdenesModificar = permisoOrdenesModificar
            bu.permisoOrdenesVer = permisoOrdenesVer
            bu.permisoPersonasCrear = permisoPersonasCrear
            bu.permisoPersonasEliminar = permisoPersonasEliminar
            bu.permisoPersonasModificar = permisoPersonasModificar
            bu.permisoPersonasVer = permisoPersonasVer
            bu.permisoProveedoresCrear = permisoProveedoresCrear
            bu.permisoProveedoresEliminar = permisoProveedoresEliminar
            bu.permisoProveedoresModificar = permisoProveedoresModificar
            bu.permisoProveedoresVer = permisoProveedoresVer
            bu.permisoProyectosCrear = permisoProyectosCrear
            bu.permisoProyectosEliminar = permisoProyectosEliminar
            bu.permisoProyectosModificar = permisoProyectosModificar
            bu.permisoProyectosPlanos = permisoProyectosPlanos
            bu.permisoProyectosVer = permisoProyectosVer
            bu.permisoResumenDeTarjetasCrear = permisoResumenDeTarjetasCrear
            bu.permisoResumenDeTarjetasEliminar = permisoResumenDeTarjetasEliminar
            bu.permisoResumenDeTarjetasModificar = permisoResumenDeTarjetasModificar
            bu.permisoResumenDeTarjetasVer = permisoResumenDeTarjetasVer
            bu.permisoValesDeGasolinaCrear = permisoValesDeGasolinaCrear
            bu.permisoValesDeGasolinaEliminar = permisoValesDeGasolinaEliminar
            bu.permisoValesDeGasolinaModificar = permisoValesDeGasolinaModificar
            bu.permisoValesDeGasolinaVer = permisoValesDeGasolinaVer

            bu.querystring = txtUnidadDeMedida.Text

            bu.isEdit = False

            bu.ShowDialog(Me)

            If bu.DialogResult = Windows.Forms.DialogResult.OK Then

                txtUnidadDeMedida.Text = bu.sunit1

                If validaInsumo(True) = False Then
                    Cursor.Current = System.Windows.Forms.Cursors.Default
                    Exit Sub
                Else
                    dgvInsumos.Enabled = True
                End If

                If IsEdit = True Then

                    Dim queries(3) As String

                    queries(0) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " SET iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "', sinputdescription = '" & txtNombreDelInsumo.Text & "', sinputunit = '" & txtUnidadDeMedida.Text & "' WHERE iinputid = " & iinputid
                    queries(1) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types SET sinputtypedescription = '" & cmbTipoDeInsumo.SelectedItem.ToString & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE iinputid = " & iinputid

                    If IsBase = True Then
                        queries(2) = "INSERT INTO logs VALUES (" & getAppDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Presupuesto Base " & iprojectid & "', 'OK')"
                    Else
                        If IsModel = True Then
                            queries(2) = "INSERT INTO logs VALUES (" & getAppDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Modelo " & iprojectid & " : " & getSQLQueryAsString(0, "SELECT smodelname FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & " WHERE imodelid = " & iprojectid) & "', 'OK')"
                        Else
                            queries(2) = "INSERT INTO logs VALUES (" & getAppDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Proyecto " & iprojectid & " : " & getSQLQueryAsString(0, "SELECT sprojectname FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & " WHERE iprojectid = " & iprojectid) & "', 'OK')"
                        End If
                    End If

                    executeTransactedSQLCommand(0, queries)

                Else

                    Dim checkIfItsOnlyTextUpdate As Boolean = False

                    checkIfItsOnlyTextUpdate = getSQLQueryAsBoolean(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " WHERE iinputid = " & iinputid)

                    If checkIfItsOnlyTextUpdate = True Then

                        Dim queries(3) As String

                        queries(0) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " SET iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "', sinputdescription = '" & txtNombreDelInsumo.Text & "', sinputunit = '" & txtUnidadDeMedida.Text & "' WHERE iinputid = " & iinputid

                        If iinputid <> 0 Then
                            queries(1) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types SET sinputtypedescription = '" & cmbTipoDeInsumo.SelectedItem.ToString & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE iinputid = " & iinputid
                        End If

                        If IsBase = True Then
                            queries(2) = "INSERT INTO logs VALUES (" & getAppDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Presupuesto Base " & iprojectid & "', 'OK')"
                        Else
                            If IsModel = True Then
                                queries(2) = "INSERT INTO logs VALUES (" & getAppDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Modelo " & iprojectid & " : " & getSQLQueryAsString(0, "SELECT smodelname FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & " WHERE imodelid = " & iprojectid) & "', 'OK')"
                            Else
                                queries(2) = "INSERT INTO logs VALUES (" & getAppDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Proyecto " & iprojectid & " : " & getSQLQueryAsString(0, "SELECT sprojectname FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & " WHERE iprojectid = " & iprojectid) & "', 'OK')"
                            End If
                        End If

                        executeTransactedSQLCommand(0, queries)

                    Else

                        Dim queries(3) As String

                        iinputid = getSQLQueryAsInteger(0, "SELECT IF(MAX(iinputid) + 1 IS NULL, 1, MAX(iinputid) + 1) AS iinputid FROM inputs ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

                        Dim queriesCreation(9) As String

                        queriesCreation(0) = "DROP TABLE IF EXISTS oversight. tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input0"
                        queriesCreation(1) = "DROP TABLE IF EXISTS oversight. tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid
                        queriesCreation(2) = "CREATE TABLE  oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " ( `iinputid` int(11) NOT NULL AUTO_INCREMENT, `sinputdescription` varchar(300) CHARACTER SET latin1 NOT NULL, `sinputunit` varchar(100) CHARACTER SET latin1 NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL,  PRIMARY KEY (`iinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

                        queriesCreation(3) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompoundInput0"
                        queriesCreation(4) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompoundInput" & iinputid
                        queriesCreation(5) = "CREATE TABLE  oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompoundInput" & iinputid & " ( `iinputid` int(11) NOT NULL, `icompoundinputid` int(11) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`iinputid`,`icompoundinputid`) USING BTREE, KEY `inputid` (`iinputid`), KEY `compoundinputid` (`icompoundinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

                        queriesCreation(6) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input0Types"
                        queriesCreation(7) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types"
                        queriesCreation(8) = "CREATE TABLE  oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types" & " ( `iinputid` int(11) NOT NULL, `sinputtypedescription` varchar(250) CHARACTER SET latin1 NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`iinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci COMMENT='Only to differ which input is taken as Mano de Obra'"

                        executeTransactedSQLCommand(0, queriesCreation)

                        queries(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " VALUES (" & iinputid & ", '" & txtNombreDelInsumo.Text & "', '" & txtUnidadDeMedida.Text & "', " & fecha & ", '" & hora & "', '" & susername & "')"

                        queries(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types VALUES (" & iinputid & ", '" & cmbTipoDeInsumo.SelectedItem.ToString & "', " & fecha & ", '" & hora & "', '" & susername & "')"

                        If IsBase = True Then
                            queries(2) = "INSERT INTO logs VALUES (" & getAppDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Presupuesto Base " & iprojectid & "', 'OK')"
                        Else
                            If IsModel = True Then
                                queries(2) = "INSERT INTO logs VALUES (" & getAppDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Modelo " & iprojectid & " : " & getSQLQueryAsString(0, "SELECT smodelname FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & " WHERE imodelid = " & iprojectid) & "', 'OK')"
                            Else
                                queries(2) = "INSERT INTO logs VALUES (" & getAppDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Proyecto " & iprojectid & " : " & getSQLQueryAsString(0, "SELECT sprojectname FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & " WHERE iprojectid = " & iprojectid) & "', 'OK')"
                            End If
                        End If

                        executeTransactedSQLCommand(0, queries)

                    End If

                End If

            Else

                txtUnidadDeMedida.Focus()
                Exit Sub

            End If

        Else


            If validaInsumo(True) = False Then
                Cursor.Current = System.Windows.Forms.Cursors.Default
                Exit Sub
            Else
                dgvInsumos.Enabled = True
            End If

            If IsEdit = True Then

                Dim queries(3) As String

                queries(0) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " SET iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "', sinputdescription = '" & txtNombreDelInsumo.Text & "', sinputunit = '" & txtUnidadDeMedida.Text & "' WHERE iinputid = " & iinputid
                queries(1) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types SET sinputtypedescription = '" & cmbTipoDeInsumo.SelectedItem.ToString & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE iinputid = " & iinputid

                If IsBase = True Then
                    queries(2) = "INSERT INTO logs VALUES (" & getAppDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Presupuesto Base " & iprojectid & "', 'OK')"
                Else
                    If IsModel = True Then
                        queries(2) = "INSERT INTO logs VALUES (" & getAppDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Modelo " & iprojectid & " : " & getSQLQueryAsString(0, "SELECT smodelname FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & " WHERE imodelid = " & iprojectid) & "', 'OK')"
                    Else
                        queries(2) = "INSERT INTO logs VALUES (" & getAppDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Proyecto " & iprojectid & " : " & getSQLQueryAsString(0, "SELECT sprojectname FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & " WHERE iprojectid = " & iprojectid) & "', 'OK')"
                    End If
                End If

                executeTransactedSQLCommand(0, queries)

            Else

                Dim checkIfItsOnlyTextUpdate As Boolean = False

                checkIfItsOnlyTextUpdate = getSQLQueryAsBoolean(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " WHERE iinputid = " & iinputid)

                If checkIfItsOnlyTextUpdate = True Then

                    Dim queries(3) As String

                    queries(0) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " SET iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "', sinputdescription = '" & txtNombreDelInsumo.Text & "', sinputunit = '" & txtUnidadDeMedida.Text & "' WHERE iinputid = " & iinputid

                    If iinputid <> 0 Then
                        queries(1) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types SET sinputtypedescription = '" & cmbTipoDeInsumo.SelectedItem.ToString & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE iinputid = " & iinputid
                    End If

                    If IsBase = True Then
                        queries(2) = "INSERT INTO logs VALUES (" & getAppDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Presupuesto Base " & iprojectid & "', 'OK')"
                    Else
                        If IsModel = True Then
                            queries(2) = "INSERT INTO logs VALUES (" & getAppDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Modelo " & iprojectid & " : " & getSQLQueryAsString(0, "SELECT smodelname FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & " WHERE imodelid = " & iprojectid) & "', 'OK')"
                        Else
                            queries(2) = "INSERT INTO logs VALUES (" & getAppDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Proyecto " & iprojectid & " : " & getSQLQueryAsString(0, "SELECT sprojectname FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & " WHERE iprojectid = " & iprojectid) & "', 'OK')"
                        End If
                    End If

                    executeTransactedSQLCommand(0, queries)

                Else

                    Dim queries(3) As String

                    iinputid = getSQLQueryAsInteger(0, "SELECT IF(MAX(iinputid) + 1 IS NULL, 1, MAX(iinputid) + 1) AS iinputid FROM inputs ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

                    Dim queriesCreation(9) As String

                    queriesCreation(0) = "DROP TABLE IF EXISTS oversight. tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input0"
                    queriesCreation(1) = "DROP TABLE IF EXISTS oversight. tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid
                    queriesCreation(2) = "CREATE TABLE  oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " ( `iinputid` int(11) NOT NULL AUTO_INCREMENT, `sinputdescription` varchar(300) CHARACTER SET latin1 NOT NULL, `sinputunit` varchar(100) CHARACTER SET latin1 NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL,  PRIMARY KEY (`iinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

                    queriesCreation(3) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompoundInput0"
                    queriesCreation(4) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompoundInput" & iinputid
                    queriesCreation(5) = "CREATE TABLE  oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompoundInput" & iinputid & " ( `iinputid` int(11) NOT NULL, `icompoundinputid` int(11) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`iinputid`,`icompoundinputid`) USING BTREE, KEY `inputid` (`iinputid`), KEY `compoundinputid` (`icompoundinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

                    queriesCreation(6) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input0Types"
                    queriesCreation(7) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types"
                    queriesCreation(8) = "CREATE TABLE  oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types" & " ( `iinputid` int(11) NOT NULL, `sinputtypedescription` varchar(250) CHARACTER SET latin1 NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`iinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci COMMENT='Only to differ which input is taken as Mano de Obra'"

                    executeTransactedSQLCommand(0, queriesCreation)

                    queries(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " VALUES (" & iinputid & ", '" & txtNombreDelInsumo.Text & "', '" & txtUnidadDeMedida.Text & "', " & fecha & ", '" & hora & "', '" & susername & "')"

                    queries(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types VALUES (" & iinputid & ", '" & cmbTipoDeInsumo.SelectedItem.ToString & "', " & fecha & ", '" & hora & "', '" & susername & "')"

                    If IsBase = True Then
                        queries(2) = "INSERT INTO logs VALUES (" & getAppDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Presupuesto Base " & iprojectid & "', 'OK')"
                    Else
                        If IsModel = True Then
                            queries(2) = "INSERT INTO logs VALUES (" & getAppDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Modelo " & iprojectid & " : " & getSQLQueryAsString(0, "SELECT smodelname FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & " WHERE imodelid = " & iprojectid) & "', 'OK')"
                        Else
                            queries(2) = "INSERT INTO logs VALUES (" & getAppDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Proyecto " & iprojectid & " : " & getSQLQueryAsString(0, "SELECT sprojectname FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & " WHERE iprojectid = " & iprojectid) & "', 'OK')"
                        End If
                    End If

                    executeTransactedSQLCommand(0, queries)

                End If

            End If


        End If



        'Inicia código del Botón Nuevo Insumo



        Dim bipni As New BuscaInsumosPreguntaTipoNuevoInsumo
        bipni.susername = susername
        bipni.bactive = bactive
        bipni.bonline = bonline
        bipni.suserfirstname = suserfirstname
        bipni.suserlastname = suserlastname
        bipni.suseremail = suseremail
        bipni.susersession = susersession
        bipni.susermachinename = susermachinename
        bipni.suserip = suserip
        bipni.permisoActivosCrear = permisoActivosCrear
        bipni.permisoActivosEliminar = permisoActivosEliminar
        bipni.permisoActivosModificar = permisoActivosModificar
        bipni.permisoActivosVer = permisoActivosVer
        bipni.permisoAnalisisDeUtilidadesCrear = permisoAnalisisDeUtilidadesCrear
        bipni.permisoAnalisisDeUtilidadesEliminar = permisoAnalisisDeUtilidadesEliminar
        bipni.permisoAnalisisDeUtilidadesModificar = permisoAnalisisDeUtilidadesModificar
        bipni.permisoAnalisisDeUtilidadesVer = permisoAnalisisDeUtilidadesVer
        bipni.permisoPersonasCrear = permisoPersonasCrear
        bipni.permisoPersonasEliminar = permisoPersonasEliminar
        bipni.permisoPersonasModificar = permisoPersonasModificar
        bipni.permisoPersonasVer = permisoPersonasVer
        bipni.permisoCotizacionesCrear = permisoCotizacionesCrear
        bipni.permisoCotizacionesEliminar = permisoCotizacionesEliminar
        bipni.permisoCotizacionesModificar = permisoCotizacionesModificar
        bipni.permisoCotizacionesVer = permisoCotizacionesVer
        bipni.permisoEnviosCrear = permisoEnviosCrear
        bipni.permisoEnviosEliminar = permisoEnviosEliminar
        bipni.permisoEnviosModificar = permisoEnviosModificar
        bipni.permisoEnviosVer = permisoEnviosVer
        bipni.permisoEquivalenciasCrear = permisoEquivalenciasCrear
        bipni.permisoEquivalenciasEliminar = permisoEquivalenciasEliminar
        bipni.permisoEquivalenciasModificar = permisoEquivalenciasModificar
        bipni.permisoEquivalenciasVer = permisoEquivalenciasVer
        bipni.permisoFacturasMRDCrear = permisoFacturasMRDCrear
        bipni.permisoFacturasMRDEliminar = permisoFacturasMRDEliminar
        bipni.permisoFacturasMRDModificar = permisoFacturasMRDModificar
        bipni.permisoFacturasMRDVer = permisoFacturasMRDVer
        bipni.permisoGastosAsignarAActivo = permisoGastosAsignarAActivo
        bipni.permisoGastosAsignarAProyecto = permisoGastosAsignarAProyecto
        bipni.permisoGastosCrear = permisoGastosCrear
        bipni.permisoGastosEliminar = permisoGastosEliminar
        bipni.permisoGastosModificar = permisoGastosModificar
        bipni.permisoGastosVer = permisoGastosVer
        bipni.permisoIngresosCrear = permisoIngresosCrear
        bipni.permisoIngresosEliminar = permisoIngresosEliminar
        bipni.permisoIngresosModificar = permisoIngresosModificar
        bipni.permisoIngresosVer = permisoIngresosVer
        bipni.permisoInsumosCrear = permisoInsumosCrear
        bipni.permisoInsumosEliminar = permisoInsumosEliminar
        bipni.permisoInsumosModificar = permisoInsumosModificar
        bipni.permisoInsumosVer = permisoInsumosVer
        bipni.permisoInventarioCrear = permisoInventarioCrear
        bipni.permisoInventarioEliminar = permisoInventarioEliminar
        bipni.permisoInventarioModificar = permisoInventarioModificar
        bipni.permisoInventarioVer = permisoInventarioVer
        bipni.permisoLogsEliminar = permisoLogsEliminar
        bipni.permisoLogsVer = permisoLogsVer
        bipni.permisoModelosCrear = permisoModelosCrear
        bipni.permisoModelosEliminar = permisoModelosEliminar
        bipni.permisoModelosModificar = permisoModelosModificar
        bipni.permisoModelosPlanos = permisoModelosPlanos
        bipni.permisoModelosVer = permisoModelosVer
        bipni.permisoOrdenesCrear = permisoOrdenesCrear
        bipni.permisoOrdenesEliminar = permisoOrdenesEliminar
        bipni.permisoOrdenesModificar = permisoOrdenesModificar
        bipni.permisoOrdenesVer = permisoOrdenesVer
        bipni.permisoPersonasCrear = permisoPersonasCrear
        bipni.permisoPersonasEliminar = permisoPersonasEliminar
        bipni.permisoPersonasModificar = permisoPersonasModificar
        bipni.permisoPersonasVer = permisoPersonasVer
        bipni.permisoProveedoresCrear = permisoProveedoresCrear
        bipni.permisoProveedoresEliminar = permisoProveedoresEliminar
        bipni.permisoProveedoresModificar = permisoProveedoresModificar
        bipni.permisoProveedoresVer = permisoProveedoresVer
        bipni.permisoProyectosCrear = permisoProyectosCrear
        bipni.permisoProyectosEliminar = permisoProyectosEliminar
        bipni.permisoProyectosModificar = permisoProyectosModificar
        bipni.permisoProyectosPlanos = permisoProyectosPlanos
        bipni.permisoProyectosVer = permisoProyectosVer
        bipni.permisoResumenDeTarjetasCrear = permisoResumenDeTarjetasCrear
        bipni.permisoResumenDeTarjetasEliminar = permisoResumenDeTarjetasEliminar
        bipni.permisoResumenDeTarjetasModificar = permisoResumenDeTarjetasModificar
        bipni.permisoResumenDeTarjetasVer = permisoResumenDeTarjetasVer
        bipni.permisoValesDeGasolinaCrear = permisoValesDeGasolinaCrear
        bipni.permisoValesDeGasolinaEliminar = permisoValesDeGasolinaEliminar
        bipni.permisoValesDeGasolinaModificar = permisoValesDeGasolinaModificar
        bipni.permisoValesDeGasolinaVer = permisoValesDeGasolinaVer

        bipni.ShowDialog(Me)

        If bipni.DialogResult = Windows.Forms.DialogResult.OK Then

            If bipni.iselectedoption = 1 Then

                'Nuevo Insumo Normal

                Dim ai As New AgregarInsumo
                ai.susername = susername
                ai.bactive = bactive
                ai.bonline = bonline
                ai.suserfirstname = suserfirstname
                ai.suserlastname = suserlastname
                ai.suseremail = suseremail
                ai.susersession = susersession
                ai.susermachinename = susermachinename
                ai.suserip = suserip
                ai.permisoActivosCrear = permisoActivosCrear
                ai.permisoActivosEliminar = permisoActivosEliminar
                ai.permisoActivosModificar = permisoActivosModificar
                ai.permisoActivosVer = permisoActivosVer
                ai.permisoAnalisisDeUtilidadesCrear = permisoAnalisisDeUtilidadesCrear
                ai.permisoAnalisisDeUtilidadesEliminar = permisoAnalisisDeUtilidadesEliminar
                ai.permisoAnalisisDeUtilidadesModificar = permisoAnalisisDeUtilidadesModificar
                ai.permisoAnalisisDeUtilidadesVer = permisoAnalisisDeUtilidadesVer
                ai.permisoPersonasCrear = permisoPersonasCrear
                ai.permisoPersonasEliminar = permisoPersonasEliminar
                ai.permisoPersonasModificar = permisoPersonasModificar
                ai.permisoPersonasVer = permisoPersonasVer
                ai.permisoCotizacionesCrear = permisoCotizacionesCrear
                ai.permisoCotizacionesEliminar = permisoCotizacionesEliminar
                ai.permisoCotizacionesModificar = permisoCotizacionesModificar
                ai.permisoCotizacionesVer = permisoCotizacionesVer
                ai.permisoEnviosCrear = permisoEnviosCrear
                ai.permisoEnviosEliminar = permisoEnviosEliminar
                ai.permisoEnviosModificar = permisoEnviosModificar
                ai.permisoEnviosVer = permisoEnviosVer
                ai.permisoEquivalenciasCrear = permisoEquivalenciasCrear
                ai.permisoEquivalenciasEliminar = permisoEquivalenciasEliminar
                ai.permisoEquivalenciasModificar = permisoEquivalenciasModificar
                ai.permisoEquivalenciasVer = permisoEquivalenciasVer
                ai.permisoFacturasMRDCrear = permisoFacturasMRDCrear
                ai.permisoFacturasMRDEliminar = permisoFacturasMRDEliminar
                ai.permisoFacturasMRDModificar = permisoFacturasMRDModificar
                ai.permisoFacturasMRDVer = permisoFacturasMRDVer
                ai.permisoGastosAsignarAActivo = permisoGastosAsignarAActivo
                ai.permisoGastosAsignarAProyecto = permisoGastosAsignarAProyecto
                ai.permisoGastosCrear = permisoGastosCrear
                ai.permisoGastosEliminar = permisoGastosEliminar
                ai.permisoGastosModificar = permisoGastosModificar
                ai.permisoGastosVer = permisoGastosVer
                ai.permisoIngresosCrear = permisoIngresosCrear
                ai.permisoIngresosEliminar = permisoIngresosEliminar
                ai.permisoIngresosModificar = permisoIngresosModificar
                ai.permisoIngresosVer = permisoIngresosVer
                ai.permisoInsumosCrear = permisoInsumosCrear
                ai.permisoInsumosEliminar = permisoInsumosEliminar
                ai.permisoInsumosModificar = permisoInsumosModificar
                ai.permisoInsumosVer = permisoInsumosVer
                ai.permisoInventarioCrear = permisoInventarioCrear
                ai.permisoInventarioEliminar = permisoInventarioEliminar
                ai.permisoInventarioModificar = permisoInventarioModificar
                ai.permisoInventarioVer = permisoInventarioVer
                ai.permisoLogsEliminar = permisoLogsEliminar
                ai.permisoLogsVer = permisoLogsVer
                ai.permisoModelosCrear = permisoModelosCrear
                ai.permisoModelosEliminar = permisoModelosEliminar
                ai.permisoModelosModificar = permisoModelosModificar
                ai.permisoModelosPlanos = permisoModelosPlanos
                ai.permisoModelosVer = permisoModelosVer
                ai.permisoOrdenesCrear = permisoOrdenesCrear
                ai.permisoOrdenesEliminar = permisoOrdenesEliminar
                ai.permisoOrdenesModificar = permisoOrdenesModificar
                ai.permisoOrdenesVer = permisoOrdenesVer
                ai.permisoPersonasCrear = permisoPersonasCrear
                ai.permisoPersonasEliminar = permisoPersonasEliminar
                ai.permisoPersonasModificar = permisoPersonasModificar
                ai.permisoPersonasVer = permisoPersonasVer
                ai.permisoProveedoresCrear = permisoProveedoresCrear
                ai.permisoProveedoresEliminar = permisoProveedoresEliminar
                ai.permisoProveedoresModificar = permisoProveedoresModificar
                ai.permisoProveedoresVer = permisoProveedoresVer
                ai.permisoProyectosCrear = permisoProyectosCrear
                ai.permisoProyectosEliminar = permisoProyectosEliminar
                ai.permisoProyectosModificar = permisoProyectosModificar
                ai.permisoProyectosPlanos = permisoProyectosPlanos
                ai.permisoProyectosVer = permisoProyectosVer
                ai.permisoResumenDeTarjetasCrear = permisoResumenDeTarjetasCrear
                ai.permisoResumenDeTarjetasEliminar = permisoResumenDeTarjetasEliminar
                ai.permisoResumenDeTarjetasModificar = permisoResumenDeTarjetasModificar
                ai.permisoResumenDeTarjetasVer = permisoResumenDeTarjetasVer
                ai.permisoValesDeGasolinaCrear = permisoValesDeGasolinaCrear
                ai.permisoValesDeGasolinaEliminar = permisoValesDeGasolinaEliminar
                ai.permisoValesDeGasolinaModificar = permisoValesDeGasolinaModificar
                ai.permisoValesDeGasolinaVer = permisoValesDeGasolinaVer

                ai.iprojectid = iprojectid
                ai.icardid = icardid

                ai.IsEdit = False
                ai.IsHistoric = False
                ai.IsModel = IsModel
                ai.IsBase = IsBase

                ai.ShowDialog(Me)

                If ai.DialogResult = Windows.Forms.DialogResult.OK Then

                    Dim dsBusquedaInsumosRepetidos As DataSet

                    If IsBase = True Then

                        dsBusquedaInsumosRepetidos = getSQLQueryAsDataset(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs WHERE ibaseid = " & baseid & " AND icardid = " & icardid & " AND icompoundinputid = " & ai.iinputid)

                    Else

                        If IsModel = True Then

                            dsBusquedaInsumosRepetidos = getSQLQueryAsDataset(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs WHERE imodelid = " & iprojectid & " AND icardid = " & icardid & " AND icompoundinputid = " & ai.iinputid)

                        Else

                            dsBusquedaInsumosRepetidos = getSQLQueryAsDataset(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs WHERE iprojectid = " & iprojectid & " AND icardid = " & icardid & " AND icompoundinputid = " & ai.iinputid)

                        End If

                    End If


                    If dsBusquedaInsumosRepetidos.Tables(0).Rows.Count > 0 Then

                        MsgBox("Ya tienes ese Insumo insertado en este Insumo Compuesto. ¿Podrías buscarlo en la lista y cambiar la cantidad si así lo deseas?", MsgBoxStyle.OkOnly, "Insumo Repetido (Err 83)")
                        dgvInsumos.EndEdit()
                        Exit Sub

                    End If

                    Dim cantidaddeinsumo As Double = 1

                    If IsBase = True Then

                        Dim queries(2) As String

                        queries(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompoundInput" & iinputid & " VALUES (" & iinputid & ", " & ai.iinputid & ", " & fecha & ", '" & hora & "', '" & susername & "')"
                        queries(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs SELECT " & baseid & ", " & icardid & ", " & ai.iinputid & ", sinputunit, 1, " & fecha & ", '" & hora & "', '" & susername & "' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " WHERE iinputid = " & ai.iinputid

                        executeTransactedSQLCommand(0, queries)

                    Else

                        If IsModel = True Then

                            Dim queries(2) As String

                            queries(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompoundInput" & iinputid & " VALUES (" & iinputid & ", " & ai.iinputid & ", " & fecha & ", '" & hora & "', '" & susername & "')"
                            queries(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs SELECT " & iprojectid & ", " & icardid & ", " & ai.iinputid & ", sinputunit, 1, " & fecha & ", '" & hora & "', '" & susername & "' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " WHERE iinputid = " & ai.iinputid

                            executeTransactedSQLCommand(0, queries)

                        Else

                            Dim queries(2) As String

                            queries(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompoundInput" & iinputid & " VALUES (" & iinputid & ", " & ai.iinputid & ", " & fecha & ", '" & hora & "', '" & susername & "')"
                            queries(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs SELECT " & iprojectid & ", " & icardid & ", " & ai.iinputid & ", sinputunit, 1, " & fecha & ", '" & hora & "', '" & susername & "' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " WHERE iinputid = " & ai.iinputid

                            executeTransactedSQLCommand(0, queries)

                        End If

                    End If

                    Dim querySumaInsumoCompuesto As String
                    Dim sumaInsumoCompuesto As Double

                    If IsBase = True Then

                        querySumaInsumoCompuesto = "" & _
                        "SELECT SUM(btfci.dcompoundinputqty*bp.dinputfinalprice) AS precio " & _
                        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs btfci " & _
                        "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompoundInput" & iinputid & " ci ON ci.icompoundinputid = btfci.icompoundinputid " & _
                        "LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " i ON i.iinputid = btfci.icompoundinputid " & _
                        "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfci.ibaseid = bp.ibaseid AND btfci.icompoundinputid = bp.iinputid " & _
                        "WHERE btfci.ibaseid = " & iprojectid & " and btfci.icardid = " & icardid & " " & _
                        "GROUP BY ci.iinputid "

                    Else

                        If IsModel = True Then

                            querySumaInsumoCompuesto = "" & _
                            "SELECT SUM(mtfci.dcompoundinputqty*mp.dinputfinalprice) AS precio " & _
                            "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs mtfci " & _
                            "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompoundInput" & iinputid & " ci ON ci.icompoundinputid = mtfci.icompoundinputid " & _
                            "LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " i ON i.iinputid = mtfci.icompoundinputid " & _
                            "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) mp GROUP BY iinputid, imodelid) mp ON mtfci.imodelid = mp.imodelid AND mtfci.icompoundinputid = mp.iinputid " & _
                            "WHERE mtfci.imodelid = " & iprojectid & " and mtfci.icardid = " & icardid & " " & _
                            "GROUP BY ci.iinputid "

                        Else

                            querySumaInsumoCompuesto = "" & _
                            "SELECT SUM(ptfci.dcompoundinputqty*pp.dinputfinalprice) AS precio " & _
                            "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs ptfci " & _
                            "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompoundInput" & iinputid & " ci ON ci.icompoundinputid = ptfci.icompoundinputid " & _
                            "LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " i ON i.iinputid = ptfci.icompoundinputid " & _
                            "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, iprojectid) pp ON ptfci.iprojectid = pp.iprojectid AND ptfci.icompoundinputid = pp.iinputid " & _
                            "WHERE ptfci.iprojectid = " & iprojectid & " and ptfci.icardid = " & icardid & " " & _
                            "GROUP BY ci.iinputid "

                        End If

                    End If

                    sumaInsumoCompuesto = getSQLQueryAsDouble(0, querySumaInsumoCompuesto)

                    txtCostoParaTabulador.Text = FormatCurrency(sumaInsumoCompuesto, 2, TriState.True, TriState.False, TriState.True)

                End If

            ElseIf bipni.iselectedoption = 2 Then

                Dim aic As New AgregarInsumoCompuesto2
                aic.susername = susername
                aic.bactive = bactive
                aic.bonline = bonline
                aic.suserfirstname = suserfirstname
                aic.suserlastname = suserlastname
                aic.suseremail = suseremail
                aic.susersession = susersession
                aic.susermachinename = susermachinename
                aic.suserip = suserip
                aic.permisoActivosCrear = permisoActivosCrear
                aic.permisoActivosEliminar = permisoActivosEliminar
                aic.permisoActivosModificar = permisoActivosModificar
                aic.permisoActivosVer = permisoActivosVer
                aic.permisoAnalisisDeUtilidadesCrear = permisoAnalisisDeUtilidadesCrear
                aic.permisoAnalisisDeUtilidadesEliminar = permisoAnalisisDeUtilidadesEliminar
                aic.permisoAnalisisDeUtilidadesModificar = permisoAnalisisDeUtilidadesModificar
                aic.permisoAnalisisDeUtilidadesVer = permisoAnalisisDeUtilidadesVer
                aic.permisoPersonasCrear = permisoPersonasCrear
                aic.permisoPersonasEliminar = permisoPersonasEliminar
                aic.permisoPersonasModificar = permisoPersonasModificar
                aic.permisoPersonasVer = permisoPersonasVer
                aic.permisoCotizacionesCrear = permisoCotizacionesCrear
                aic.permisoCotizacionesEliminar = permisoCotizacionesEliminar
                aic.permisoCotizacionesModificar = permisoCotizacionesModificar
                aic.permisoCotizacionesVer = permisoCotizacionesVer
                aic.permisoEnviosCrear = permisoEnviosCrear
                aic.permisoEnviosEliminar = permisoEnviosEliminar
                aic.permisoEnviosModificar = permisoEnviosModificar
                aic.permisoEnviosVer = permisoEnviosVer
                aic.permisoEquivalenciasCrear = permisoEquivalenciasCrear
                aic.permisoEquivalenciasEliminar = permisoEquivalenciasEliminar
                aic.permisoEquivalenciasModificar = permisoEquivalenciasModificar
                aic.permisoEquivalenciasVer = permisoEquivalenciasVer
                aic.permisoFacturasMRDCrear = permisoFacturasMRDCrear
                aic.permisoFacturasMRDEliminar = permisoFacturasMRDEliminar
                aic.permisoFacturasMRDModificar = permisoFacturasMRDModificar
                aic.permisoFacturasMRDVer = permisoFacturasMRDVer
                aic.permisoGastosAsignarAActivo = permisoGastosAsignarAActivo
                aic.permisoGastosAsignarAProyecto = permisoGastosAsignarAProyecto
                aic.permisoGastosCrear = permisoGastosCrear
                aic.permisoGastosEliminar = permisoGastosEliminar
                aic.permisoGastosModificar = permisoGastosModificar
                aic.permisoGastosVer = permisoGastosVer
                aic.permisoIngresosCrear = permisoIngresosCrear
                aic.permisoIngresosEliminar = permisoIngresosEliminar
                aic.permisoIngresosModificar = permisoIngresosModificar
                aic.permisoIngresosVer = permisoIngresosVer
                aic.permisoInsumosCrear = permisoInsumosCrear
                aic.permisoInsumosEliminar = permisoInsumosEliminar
                aic.permisoInsumosModificar = permisoInsumosModificar
                aic.permisoInsumosVer = permisoInsumosVer
                aic.permisoInventarioCrear = permisoInventarioCrear
                aic.permisoInventarioEliminar = permisoInventarioEliminar
                aic.permisoInventarioModificar = permisoInventarioModificar
                aic.permisoInventarioVer = permisoInventarioVer
                aic.permisoLogsEliminar = permisoLogsEliminar
                aic.permisoLogsVer = permisoLogsVer
                aic.permisoModelosCrear = permisoModelosCrear
                aic.permisoModelosEliminar = permisoModelosEliminar
                aic.permisoModelosModificar = permisoModelosModificar
                aic.permisoModelosPlanos = permisoModelosPlanos
                aic.permisoModelosVer = permisoModelosVer
                aic.permisoOrdenesCrear = permisoOrdenesCrear
                aic.permisoOrdenesEliminar = permisoOrdenesEliminar
                aic.permisoOrdenesModificar = permisoOrdenesModificar
                aic.permisoOrdenesVer = permisoOrdenesVer
                aic.permisoPersonasCrear = permisoPersonasCrear
                aic.permisoPersonasEliminar = permisoPersonasEliminar
                aic.permisoPersonasModificar = permisoPersonasModificar
                aic.permisoPersonasVer = permisoPersonasVer
                aic.permisoProveedoresCrear = permisoProveedoresCrear
                aic.permisoProveedoresEliminar = permisoProveedoresEliminar
                aic.permisoProveedoresModificar = permisoProveedoresModificar
                aic.permisoProveedoresVer = permisoProveedoresVer
                aic.permisoProyectosCrear = permisoProyectosCrear
                aic.permisoProyectosEliminar = permisoProyectosEliminar
                aic.permisoProyectosModificar = permisoProyectosModificar
                aic.permisoProyectosPlanos = permisoProyectosPlanos
                aic.permisoProyectosVer = permisoProyectosVer
                aic.permisoResumenDeTarjetasCrear = permisoResumenDeTarjetasCrear
                aic.permisoResumenDeTarjetasEliminar = permisoResumenDeTarjetasEliminar
                aic.permisoResumenDeTarjetasModificar = permisoResumenDeTarjetasModificar
                aic.permisoResumenDeTarjetasVer = permisoResumenDeTarjetasVer
                aic.permisoValesDeGasolinaCrear = permisoValesDeGasolinaCrear
                aic.permisoValesDeGasolinaEliminar = permisoValesDeGasolinaEliminar
                aic.permisoValesDeGasolinaModificar = permisoValesDeGasolinaModificar
                aic.permisoValesDeGasolinaVer = permisoValesDeGasolinaVer

                aic.iprojectid = iprojectid
                aic.icardid = icardid

                aic.IsEdit = False
                aic.IsHistoric = False
                aic.IsModel = IsModel
                aic.IsBase = IsBase

                aic.ShowDialog(Me)

                If aic.DialogResult = Windows.Forms.DialogResult.OK Then

                    Dim dsBusquedaInsumosRepetidos As DataSet

                    If IsBase = True Then

                        dsBusquedaInsumosRepetidos = getSQLQueryAsDataset(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs WHERE ibaseid = " & baseid & " AND icardid = " & icardid & " AND icompoundinputid = " & aic.iinputid)

                    Else

                        If IsModel = True Then

                            dsBusquedaInsumosRepetidos = getSQLQueryAsDataset(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs WHERE imodelid = " & iprojectid & " AND icardid = " & icardid & " AND icompoundinputid = " & aic.iinputid)

                        Else

                            dsBusquedaInsumosRepetidos = getSQLQueryAsDataset(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs WHERE iprojectid = " & iprojectid & " AND icardid = " & icardid & " AND icompoundinputid = " & aic.iinputid)

                        End If

                    End If


                    If dsBusquedaInsumosRepetidos.Tables(0).Rows.Count > 0 Then

                        MsgBox("Ya tienes ese Insumo insertado en este Insumo Compuesto. ¿Podrías buscarlo en la lista y cambiar la cantidad si así lo deseas?", MsgBoxStyle.OkOnly, "Insumo Repetido (Err 83)")
                        dgvInsumos.EndEdit()
                        Exit Sub

                    End If

                    Dim cantidaddeinsumo As Double = 1

                    If IsBase = True Then

                        Dim queries(2) As String

                        queries(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompoundInput" & iinputid & " VALUES (" & iinputid & ", " & aic.iinputid & ", " & fecha & ", '" & hora & "', '" & susername & "')"
                        queries(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs SELECT " & baseid & ", " & icardid & ", " & aic.iinputid & ", sinputunit, 1, " & fecha & ", '" & hora & "', '" & susername & "' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " WHERE iinputid = " & aic.iinputid

                        executeTransactedSQLCommand(0, queries)

                    Else

                        If IsModel = True Then

                            Dim queries(2) As String

                            queries(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompoundInput" & iinputid & " VALUES (" & iinputid & ", " & aic.iinputid & ", " & fecha & ", '" & hora & "', '" & susername & "')"
                            queries(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs SELECT " & iprojectid & ", " & icardid & ", " & aic.iinputid & ", sinputunit, 1, " & fecha & ", '" & hora & "', '" & susername & "' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " WHERE iinputid = " & aic.iinputid

                            executeTransactedSQLCommand(0, queries)

                        Else

                            Dim queries(2) As String

                            queries(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompoundInput" & iinputid & " VALUES (" & iinputid & ", " & aic.iinputid & ", " & fecha & ", '" & hora & "', '" & susername & "')"
                            queries(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs SELECT " & iprojectid & ", " & icardid & ", " & aic.iinputid & ", sinputunit, 1, " & fecha & ", '" & hora & "', '" & susername & "' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " WHERE iinputid = " & aic.iinputid

                            executeTransactedSQLCommand(0, queries)

                        End If

                    End If

                    Dim querySumaInsumoCompuesto As String
                    Dim sumaInsumoCompuesto As Double

                    If IsBase = True Then

                        querySumaInsumoCompuesto = "" & _
                        "SELECT SUM(btfci.dcompoundinputqty*bp.dinputfinalprice) AS precio " & _
                        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs btfci " & _
                        "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompoundInput" & iinputid & " ci ON ci.icompoundinputid = btfci.icompoundinputid " & _
                        "LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " i ON i.iinputid = btfci.icompoundinputid " & _
                        "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfci.ibaseid = bp.ibaseid AND btfci.icompoundinputid = bp.iinputid " & _
                        "WHERE btfci.ibaseid = " & iprojectid & " and btfci.icardid = " & icardid & " " & _
                        "GROUP BY ci.iinputid "

                    Else

                        If IsModel = True Then

                            querySumaInsumoCompuesto = "" & _
                            "SELECT SUM(mtfci.dcompoundinputqty*mp.dinputfinalprice) AS precio " & _
                            "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs mtfci " & _
                            "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompoundInput" & iinputid & " ci ON ci.icompoundinputid = mtfci.icompoundinputid " & _
                            "LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " i ON i.iinputid = mtfci.icompoundinputid " & _
                            "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) mp GROUP BY iinputid, imodelid) mp ON mtfci.imodelid = mp.imodelid AND mtfci.icompoundinputid = mp.iinputid " & _
                            "WHERE mtfci.imodelid = " & iprojectid & " and mtfci.icardid = " & icardid & " " & _
                            "GROUP BY ci.iinputid "

                        Else

                            querySumaInsumoCompuesto = "" & _
                            "SELECT SUM(ptfci.dcompoundinputqty*pp.dinputfinalprice) AS precio " & _
                            "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs ptfci " & _
                            "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompoundInput" & iinputid & " ci ON ci.icompoundinputid = ptfci.icompoundinputid " & _
                            "LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " i ON i.iinputid = ptfci.icompoundinputid " & _
                            "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, iprojectid) pp ON ptfci.iprojectid = pp.iprojectid AND ptfci.icompoundinputid = pp.iinputid " & _
                            "WHERE ptfci.iprojectid = " & iprojectid & " and ptfci.icardid = " & icardid & " " & _
                            "GROUP BY ci.iinputid "

                        End If

                    End If

                    sumaInsumoCompuesto = getSQLQueryAsDouble(0, querySumaInsumoCompuesto)

                    txtCostoParaTabulador.Text = FormatCurrency(sumaInsumoCompuesto, 2, TriState.True, TriState.False, TriState.True)

                End If

            End If

            Dim queryInsumos As String

            If IsBase = True Then

                queryInsumos = "" & _
                "SELECT btfci.icompoundinputid, i.sinputdescription AS 'Insumo', btfci.scompoundinputunit AS 'Unidad', btfci.dcompoundinputqty AS 'Cantidad', " & _
                "FORMAT((btfci.dcompoundinputqty*bp.dinputfinalprice), 3) AS 'Precio' " & _
                "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs btfci " & _
                "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompoundInput" & iinputid & " ci ON ci.icompoundinputid = btfci.icompoundinputid " & _
                "LEFT JOIN inputs i ON i.iinputid = btfci.icompoundinputid " & _
                "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfci.ibaseid = bp.ibaseid AND btfci.icompoundinputid = bp.iinputid " & _
                "WHERE btfci.ibaseid = " & iprojectid & " AND btfci.icardid = " & icardid & " AND ci.iinputid = " & iinputid

            Else

                If IsModel = True Then

                    queryInsumos = "" & _
                    "SELECT mtfci.icompoundinputid, i.sinputdescription AS 'Insumo', mtfci.scompoundinputunit AS 'Unidad', mtfci.dcompoundinputqty AS 'Cantidad', " & _
                    "FORMAT((mtfci.dcompoundinputqty*mp.dinputfinalprice), 3) AS 'Precio' " & _
                    "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs mtfci " & _
                    "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompoundInput" & iinputid & " ci ON ci.icompoundinputid = mtfci.icompoundinputid " & _
                    "LEFT JOIN inputs i ON i.iinputid = mtfci.icompoundinputid " & _
                    "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) mp GROUP BY iinputid, imodelid) mp ON mtfci.imodelid = mp.imodelid AND mtfci.icompoundinputid = mp.iinputid " & _
                    "WHERE mtfci.imodelid = " & iprojectid & " AND mtfci.icardid = " & icardid & " AND ci.iinputid = " & iinputid

                Else

                    queryInsumos = "" & _
                    "SELECT ptfci.icompoundinputid, i.sinputdescription AS 'Insumo', ptfci.scompoundinputunit AS 'Unidad', FORMAT(ptfci.dcompoundinputqty, 3) AS 'Cantidad', " & _
                    "FORMAT((ptfci.dcompoundinputqty*pp.dinputfinalprice), 3) AS 'Precio' " & _
                    "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs ptfci " & _
                    "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompoundInput" & iinputid & " ci ON ci.icompoundinputid = ptfci.icompoundinputid " & _
                    "LEFT JOIN inputs i ON i.iinputid = ptfci.icompoundinputid " & _
                    "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, iprojectid) pp ON ptfci.iprojectid = pp.iprojectid AND ptfci.icompoundinputid = pp.iinputid " & _
                    "WHERE ptfci.iprojectid = " & iprojectid & " AND ptfci.icardid = " & icardid & " AND ci.iinputid = " & iinputid

                End If

            End If

            setDataGridView(dgvInsumos, queryInsumos, False)

            dgvInsumos.Columns(0).Visible = False

            dgvInsumos.Columns(0).ReadOnly = True
            dgvInsumos.Columns(2).ReadOnly = True
            dgvInsumos.Columns(4).ReadOnly = True

            isFormReadyForAction = True

        End If

    End Sub


    Private Sub btnInsertarInsumo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnInsertarInsumo.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor


        'Inicia Save de Master del Insumo Compuesto

        If isFormReadyForAction = False Then
            Exit Sub
        End If

        If IsHistoric = True Then
            Exit Sub
        End If

        Dim unitfound As Boolean = False
        Dim strcaracteresprohibidos As String = "|°!#$%&/()=?¡*¨[]_:;,.-{}+´¿'¬^`~@\<> "

        txtUnidadDeMedida.Text = txtUnidadDeMedida.Text.Trim.Trim(strcaracteresprohibidos.ToCharArray)
        txtUnidadDeMedida.Text = txtUnidadDeMedida.Text.ToUpper

        Dim fecha As Integer = 0
        Dim hora As String = "00:00:00"

        fecha = getAppDate()
        hora = getAppTime()

        Dim baseid As Integer = 0
        baseid = getSQLQueryAsInteger(0, "SELECT ibaseid FROM base ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

        If baseid = 0 Then
            baseid = 1
        End If

        unitfound = getSQLQueryAsBoolean(0, "SELECT count(*) FROM transformationunits WHERE soriginunit = '" & txtUnidadDeMedida.Text.Replace("--", "") & "'")

        If unitfound = False Then

            MsgBox("¡No encontré esa unidad de Medida! ¿Podrías seleccionar una de la lista?", MsgBoxStyle.OkOnly, "Unidad de Medida No Encontrada (Err 91)")

            Dim bu As New BuscaUnidades
            bu.susername = susername
            bu.bactive = bactive
            bu.bonline = bonline
            bu.suserfirstname = suserfirstname
            bu.suserlastname = suserlastname
            bu.suseremail = suseremail
            bu.susersession = susersession
            bu.susermachinename = susermachinename
            bu.suserip = suserip
            bu.permisoActivosCrear = permisoActivosCrear
            bu.permisoActivosEliminar = permisoActivosEliminar
            bu.permisoActivosModificar = permisoActivosModificar
            bu.permisoActivosVer = permisoActivosVer
            bu.permisoAnalisisDeUtilidadesCrear = permisoAnalisisDeUtilidadesCrear
            bu.permisoAnalisisDeUtilidadesEliminar = permisoAnalisisDeUtilidadesEliminar
            bu.permisoAnalisisDeUtilidadesModificar = permisoAnalisisDeUtilidadesModificar
            bu.permisoAnalisisDeUtilidadesVer = permisoAnalisisDeUtilidadesVer
            bu.permisoPersonasCrear = permisoPersonasCrear
            bu.permisoPersonasEliminar = permisoPersonasEliminar
            bu.permisoPersonasModificar = permisoPersonasModificar
            bu.permisoPersonasVer = permisoPersonasVer
            bu.permisoCotizacionesCrear = permisoCotizacionesCrear
            bu.permisoCotizacionesEliminar = permisoCotizacionesEliminar
            bu.permisoCotizacionesModificar = permisoCotizacionesModificar
            bu.permisoCotizacionesVer = permisoCotizacionesVer
            bu.permisoEnviosCrear = permisoEnviosCrear
            bu.permisoEnviosEliminar = permisoEnviosEliminar
            bu.permisoEnviosModificar = permisoEnviosModificar
            bu.permisoEnviosVer = permisoEnviosVer
            bu.permisoEquivalenciasCrear = permisoEquivalenciasCrear
            bu.permisoEquivalenciasEliminar = permisoEquivalenciasEliminar
            bu.permisoEquivalenciasModificar = permisoEquivalenciasModificar
            bu.permisoEquivalenciasVer = permisoEquivalenciasVer
            bu.permisoFacturasMRDCrear = permisoFacturasMRDCrear
            bu.permisoFacturasMRDEliminar = permisoFacturasMRDEliminar
            bu.permisoFacturasMRDModificar = permisoFacturasMRDModificar
            bu.permisoFacturasMRDVer = permisoFacturasMRDVer
            bu.permisoGastosAsignarAActivo = permisoGastosAsignarAActivo
            bu.permisoGastosAsignarAProyecto = permisoGastosAsignarAProyecto
            bu.permisoGastosCrear = permisoGastosCrear
            bu.permisoGastosEliminar = permisoGastosEliminar
            bu.permisoGastosModificar = permisoGastosModificar
            bu.permisoGastosVer = permisoGastosVer
            bu.permisoIngresosCrear = permisoIngresosCrear
            bu.permisoIngresosEliminar = permisoIngresosEliminar
            bu.permisoIngresosModificar = permisoIngresosModificar
            bu.permisoIngresosVer = permisoIngresosVer
            bu.permisoInsumosCrear = permisoInsumosCrear
            bu.permisoInsumosEliminar = permisoInsumosEliminar
            bu.permisoInsumosModificar = permisoInsumosModificar
            bu.permisoInsumosVer = permisoInsumosVer
            bu.permisoInventarioCrear = permisoInventarioCrear
            bu.permisoInventarioEliminar = permisoInventarioEliminar
            bu.permisoInventarioModificar = permisoInventarioModificar
            bu.permisoInventarioVer = permisoInventarioVer
            bu.permisoLogsEliminar = permisoLogsEliminar
            bu.permisoLogsVer = permisoLogsVer
            bu.permisoModelosCrear = permisoModelosCrear
            bu.permisoModelosEliminar = permisoModelosEliminar
            bu.permisoModelosModificar = permisoModelosModificar
            bu.permisoModelosPlanos = permisoModelosPlanos
            bu.permisoModelosVer = permisoModelosVer
            bu.permisoOrdenesCrear = permisoOrdenesCrear
            bu.permisoOrdenesEliminar = permisoOrdenesEliminar
            bu.permisoOrdenesModificar = permisoOrdenesModificar
            bu.permisoOrdenesVer = permisoOrdenesVer
            bu.permisoPersonasCrear = permisoPersonasCrear
            bu.permisoPersonasEliminar = permisoPersonasEliminar
            bu.permisoPersonasModificar = permisoPersonasModificar
            bu.permisoPersonasVer = permisoPersonasVer
            bu.permisoProveedoresCrear = permisoProveedoresCrear
            bu.permisoProveedoresEliminar = permisoProveedoresEliminar
            bu.permisoProveedoresModificar = permisoProveedoresModificar
            bu.permisoProveedoresVer = permisoProveedoresVer
            bu.permisoProyectosCrear = permisoProyectosCrear
            bu.permisoProyectosEliminar = permisoProyectosEliminar
            bu.permisoProyectosModificar = permisoProyectosModificar
            bu.permisoProyectosPlanos = permisoProyectosPlanos
            bu.permisoProyectosVer = permisoProyectosVer
            bu.permisoResumenDeTarjetasCrear = permisoResumenDeTarjetasCrear
            bu.permisoResumenDeTarjetasEliminar = permisoResumenDeTarjetasEliminar
            bu.permisoResumenDeTarjetasModificar = permisoResumenDeTarjetasModificar
            bu.permisoResumenDeTarjetasVer = permisoResumenDeTarjetasVer
            bu.permisoValesDeGasolinaCrear = permisoValesDeGasolinaCrear
            bu.permisoValesDeGasolinaEliminar = permisoValesDeGasolinaEliminar
            bu.permisoValesDeGasolinaModificar = permisoValesDeGasolinaModificar
            bu.permisoValesDeGasolinaVer = permisoValesDeGasolinaVer

            bu.querystring = txtUnidadDeMedida.Text

            bu.isEdit = False

            bu.ShowDialog(Me)

            If bu.DialogResult = Windows.Forms.DialogResult.OK Then

                txtUnidadDeMedida.Text = bu.sunit1

                If validaInsumo(True) = False Then
                    Cursor.Current = System.Windows.Forms.Cursors.Default
                    Exit Sub
                Else
                    dgvInsumos.Enabled = True
                End If

                If IsEdit = True Then

                    Dim queries(3) As String

                    queries(0) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " SET iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "', sinputdescription = '" & txtNombreDelInsumo.Text & "', sinputunit = '" & txtUnidadDeMedida.Text & "' WHERE iinputid = " & iinputid
                    queries(1) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types SET sinputtypedescription = '" & cmbTipoDeInsumo.SelectedItem.ToString & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE iinputid = " & iinputid

                    If IsBase = True Then
                        queries(2) = "INSERT INTO logs VALUES (" & getAppDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Presupuesto Base " & iprojectid & "', 'OK')"
                    Else
                        If IsModel = True Then
                            queries(2) = "INSERT INTO logs VALUES (" & getAppDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Modelo " & iprojectid & " : " & getSQLQueryAsString(0, "SELECT smodelname FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & " WHERE imodelid = " & iprojectid) & "', 'OK')"
                        Else
                            queries(2) = "INSERT INTO logs VALUES (" & getAppDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Proyecto " & iprojectid & " : " & getSQLQueryAsString(0, "SELECT sprojectname FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & " WHERE iprojectid = " & iprojectid) & "', 'OK')"
                        End If
                    End If

                    executeTransactedSQLCommand(0, queries)

                Else

                    Dim checkIfItsOnlyTextUpdate As Boolean = False

                    checkIfItsOnlyTextUpdate = getSQLQueryAsBoolean(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " WHERE iinputid = " & iinputid)

                    If checkIfItsOnlyTextUpdate = True Then

                        Dim queries(3) As String

                        queries(0) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " SET iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "', sinputdescription = '" & txtNombreDelInsumo.Text & "', sinputunit = '" & txtUnidadDeMedida.Text & "' WHERE iinputid = " & iinputid

                        If iinputid <> 0 Then
                            queries(1) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types SET sinputtypedescription = '" & cmbTipoDeInsumo.SelectedItem.ToString & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE iinputid = " & iinputid
                        End If

                        If IsBase = True Then
                            queries(2) = "INSERT INTO logs VALUES (" & getAppDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Presupuesto Base " & iprojectid & "', 'OK')"
                        Else
                            If IsModel = True Then
                                queries(2) = "INSERT INTO logs VALUES (" & getAppDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Modelo " & iprojectid & " : " & getSQLQueryAsString(0, "SELECT smodelname FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & " WHERE imodelid = " & iprojectid) & "', 'OK')"
                            Else
                                queries(2) = "INSERT INTO logs VALUES (" & getAppDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Proyecto " & iprojectid & " : " & getSQLQueryAsString(0, "SELECT sprojectname FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & " WHERE iprojectid = " & iprojectid) & "', 'OK')"
                            End If
                        End If

                        executeTransactedSQLCommand(0, queries)

                    Else

                        Dim queries(3) As String

                        iinputid = getSQLQueryAsInteger(0, "SELECT IF(MAX(iinputid) + 1 IS NULL, 1, MAX(iinputid) + 1) AS iinputid FROM inputs ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

                        Dim queriesCreation(9) As String

                        queriesCreation(0) = "DROP TABLE IF EXISTS oversight. tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input0"
                        queriesCreation(1) = "DROP TABLE IF EXISTS oversight. tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid
                        queriesCreation(2) = "CREATE TABLE  oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " ( `iinputid` int(11) NOT NULL AUTO_INCREMENT, `sinputdescription` varchar(300) CHARACTER SET latin1 NOT NULL, `sinputunit` varchar(100) CHARACTER SET latin1 NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL,  PRIMARY KEY (`iinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

                        queriesCreation(3) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompoundInput0"
                        queriesCreation(4) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompoundInput" & iinputid
                        queriesCreation(5) = "CREATE TABLE  oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompoundInput" & iinputid & " ( `iinputid` int(11) NOT NULL, `icompoundinputid` int(11) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`iinputid`,`icompoundinputid`) USING BTREE, KEY `inputid` (`iinputid`), KEY `compoundinputid` (`icompoundinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

                        queriesCreation(6) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input0Types"
                        queriesCreation(7) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types"
                        queriesCreation(8) = "CREATE TABLE  oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types" & " ( `iinputid` int(11) NOT NULL, `sinputtypedescription` varchar(250) CHARACTER SET latin1 NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`iinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci COMMENT='Only to differ which input is taken as Mano de Obra'"

                        executeTransactedSQLCommand(0, queriesCreation)

                        queries(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " VALUES (" & iinputid & ", '" & txtNombreDelInsumo.Text & "', '" & txtUnidadDeMedida.Text & "', " & fecha & ", '" & hora & "', '" & susername & "')"

                        queries(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types VALUES (" & iinputid & ", '" & cmbTipoDeInsumo.SelectedItem.ToString & "', " & fecha & ", '" & hora & "', '" & susername & "')"

                        If IsBase = True Then
                            queries(2) = "INSERT INTO logs VALUES (" & getAppDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Presupuesto Base " & iprojectid & "', 'OK')"
                        Else
                            If IsModel = True Then
                                queries(2) = "INSERT INTO logs VALUES (" & getAppDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Modelo " & iprojectid & " : " & getSQLQueryAsString(0, "SELECT smodelname FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & " WHERE imodelid = " & iprojectid) & "', 'OK')"
                            Else
                                queries(2) = "INSERT INTO logs VALUES (" & getAppDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Proyecto " & iprojectid & " : " & getSQLQueryAsString(0, "SELECT sprojectname FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & " WHERE iprojectid = " & iprojectid) & "', 'OK')"
                            End If
                        End If

                        executeTransactedSQLCommand(0, queries)

                    End If

                End If

            Else

                txtUnidadDeMedida.Focus()
                Exit Sub

            End If

        Else


            If validaInsumo(True) = False Then
                Cursor.Current = System.Windows.Forms.Cursors.Default
                Exit Sub
            Else
                dgvInsumos.Enabled = True
            End If

            If IsEdit = True Then

                Dim queries(3) As String

                queries(0) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " SET iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "', sinputdescription = '" & txtNombreDelInsumo.Text & "', sinputunit = '" & txtUnidadDeMedida.Text & "' WHERE iinputid = " & iinputid
                queries(1) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types SET sinputtypedescription = '" & cmbTipoDeInsumo.SelectedItem.ToString & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE iinputid = " & iinputid

                If IsBase = True Then
                    queries(2) = "INSERT INTO logs VALUES (" & getAppDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Presupuesto Base " & iprojectid & "', 'OK')"
                Else
                    If IsModel = True Then
                        queries(2) = "INSERT INTO logs VALUES (" & getAppDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Modelo " & iprojectid & " : " & getSQLQueryAsString(0, "SELECT smodelname FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & " WHERE imodelid = " & iprojectid) & "', 'OK')"
                    Else
                        queries(2) = "INSERT INTO logs VALUES (" & getAppDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Proyecto " & iprojectid & " : " & getSQLQueryAsString(0, "SELECT sprojectname FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & " WHERE iprojectid = " & iprojectid) & "', 'OK')"
                    End If
                End If

                executeTransactedSQLCommand(0, queries)

            Else

                Dim checkIfItsOnlyTextUpdate As Boolean = False

                checkIfItsOnlyTextUpdate = getSQLQueryAsBoolean(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " WHERE iinputid = " & iinputid)

                If checkIfItsOnlyTextUpdate = True Then

                    Dim queries(3) As String

                    queries(0) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " SET iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "', sinputdescription = '" & txtNombreDelInsumo.Text & "', sinputunit = '" & txtUnidadDeMedida.Text & "' WHERE iinputid = " & iinputid

                    If iinputid <> 0 Then
                        queries(1) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types SET sinputtypedescription = '" & cmbTipoDeInsumo.SelectedItem.ToString & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE iinputid = " & iinputid
                    End If

                    If IsBase = True Then
                        queries(2) = "INSERT INTO logs VALUES (" & getAppDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Presupuesto Base " & iprojectid & "', 'OK')"
                    Else
                        If IsModel = True Then
                            queries(2) = "INSERT INTO logs VALUES (" & getAppDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Modelo " & iprojectid & " : " & getSQLQueryAsString(0, "SELECT smodelname FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & " WHERE imodelid = " & iprojectid) & "', 'OK')"
                        Else
                            queries(2) = "INSERT INTO logs VALUES (" & getAppDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Proyecto " & iprojectid & " : " & getSQLQueryAsString(0, "SELECT sprojectname FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & " WHERE iprojectid = " & iprojectid) & "', 'OK')"
                        End If
                    End If

                    executeTransactedSQLCommand(0, queries)

                Else

                    Dim queries(3) As String

                    iinputid = getSQLQueryAsInteger(0, "SELECT IF(MAX(iinputid) + 1 IS NULL, 1, MAX(iinputid) + 1) AS iinputid FROM inputs ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

                    Dim queriesCreation(9) As String

                    queriesCreation(0) = "DROP TABLE IF EXISTS oversight. tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input0"
                    queriesCreation(1) = "DROP TABLE IF EXISTS oversight. tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid
                    queriesCreation(2) = "CREATE TABLE  oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " ( `iinputid` int(11) NOT NULL AUTO_INCREMENT, `sinputdescription` varchar(300) CHARACTER SET latin1 NOT NULL, `sinputunit` varchar(100) CHARACTER SET latin1 NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL,  PRIMARY KEY (`iinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

                    queriesCreation(3) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompoundInput0"
                    queriesCreation(4) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompoundInput" & iinputid
                    queriesCreation(5) = "CREATE TABLE  oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompoundInput" & iinputid & " ( `iinputid` int(11) NOT NULL, `icompoundinputid` int(11) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`iinputid`,`icompoundinputid`) USING BTREE, KEY `inputid` (`iinputid`), KEY `compoundinputid` (`icompoundinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

                    queriesCreation(6) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input0Types"
                    queriesCreation(7) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types"
                    queriesCreation(8) = "CREATE TABLE  oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types" & " ( `iinputid` int(11) NOT NULL, `sinputtypedescription` varchar(250) CHARACTER SET latin1 NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`iinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci COMMENT='Only to differ which input is taken as Mano de Obra'"

                    executeTransactedSQLCommand(0, queriesCreation)

                    queries(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " VALUES (" & iinputid & ", '" & txtNombreDelInsumo.Text & "', '" & txtUnidadDeMedida.Text & "', " & fecha & ", '" & hora & "', '" & susername & "')"

                    queries(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types VALUES (" & iinputid & ", '" & cmbTipoDeInsumo.SelectedItem.ToString & "', " & fecha & ", '" & hora & "', '" & susername & "')"

                    If IsBase = True Then
                        queries(2) = "INSERT INTO logs VALUES (" & getAppDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Presupuesto Base " & iprojectid & "', 'OK')"
                    Else
                        If IsModel = True Then
                            queries(2) = "INSERT INTO logs VALUES (" & getAppDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Modelo " & iprojectid & " : " & getSQLQueryAsString(0, "SELECT smodelname FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & " WHERE imodelid = " & iprojectid) & "', 'OK')"
                        Else
                            queries(2) = "INSERT INTO logs VALUES (" & getAppDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Proyecto " & iprojectid & " : " & getSQLQueryAsString(0, "SELECT sprojectname FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & " WHERE iprojectid = " & iprojectid) & "', 'OK')"
                        End If
                    End If

                    executeTransactedSQLCommand(0, queries)

                End If

            End If


        End If


        'Inicia código del Botón Insertar Insumo


        isFormReadyForAction = False

        Dim chequeoPrimeraVezQueSeInsertaAlgo As Integer = 0

        chequeoPrimeraVezQueSeInsertaAlgo = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Prices WHERE iprojectid = " & iprojectid)

        If IsBase = True And baseid <> iprojectid Then

            chequeoPrimeraVezQueSeInsertaAlgo = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices WHERE ibaseid = " & iprojectid)

            If chequeoPrimeraVezQueSeInsertaAlgo = 0 Then
                executeSQLCommand(0, "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices SELECT " & iprojectid & ", iinputid, dinputpricewithoutIVA, dinputprotectionpercentage, dinputfinalprice, " & fecha & ", '" & hora & "', '" & susername & "' FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices WHERE ibaseid = " & baseid & " ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid")
            End If

        ElseIf IsModel = True Then

            chequeoPrimeraVezQueSeInsertaAlgo = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Prices WHERE imodelid = " & iprojectid)

            If chequeoPrimeraVezQueSeInsertaAlgo = 0 Then
                executeSQLCommand(0, "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Prices SELECT " & iprojectid & ", iinputid, dinputpricewithoutIVA, dinputprotectionpercentage, dinputfinalprice, " & fecha & ", '" & hora & "', '" & susername & "' FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices WHERE ibaseid = " & baseid & " ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid")
            End If

        ElseIf IsModel = False And IsBase = False Then

            chequeoPrimeraVezQueSeInsertaAlgo = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Prices WHERE iprojectid = " & iprojectid)

            If chequeoPrimeraVezQueSeInsertaAlgo = 0 Then
                executeSQLCommand(0, "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Prices SELECT " & iprojectid & ", iinputid, dinputpricewithoutIVA, dinputprotectionpercentage, dinputfinalprice, " & fecha & ", '" & hora & "', '" & susername & "' FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices WHERE ibaseid = " & baseid & " ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid")
            End If

        End If

        Dim bi As New BuscaInsumos
        bi.susername = susername
        bi.bactive = bactive
        bi.bonline = bonline
        bi.suserfirstname = suserfirstname
        bi.suserlastname = suserlastname
        bi.suseremail = suseremail
        bi.susersession = susersession
        bi.susermachinename = susermachinename
        bi.suserip = suserip
        bi.permisoActivosCrear = permisoActivosCrear
        bi.permisoActivosEliminar = permisoActivosEliminar
        bi.permisoActivosModificar = permisoActivosModificar
        bi.permisoActivosVer = permisoActivosVer
        bi.permisoAnalisisDeUtilidadesCrear = permisoAnalisisDeUtilidadesCrear
        bi.permisoAnalisisDeUtilidadesEliminar = permisoAnalisisDeUtilidadesEliminar
        bi.permisoAnalisisDeUtilidadesModificar = permisoAnalisisDeUtilidadesModificar
        bi.permisoAnalisisDeUtilidadesVer = permisoAnalisisDeUtilidadesVer
        bi.permisoPersonasCrear = permisoPersonasCrear
        bi.permisoPersonasEliminar = permisoPersonasEliminar
        bi.permisoPersonasModificar = permisoPersonasModificar
        bi.permisoPersonasVer = permisoPersonasVer
        bi.permisoCotizacionesCrear = permisoCotizacionesCrear
        bi.permisoCotizacionesEliminar = permisoCotizacionesEliminar
        bi.permisoCotizacionesModificar = permisoCotizacionesModificar
        bi.permisoCotizacionesVer = permisoCotizacionesVer
        bi.permisoEnviosCrear = permisoEnviosCrear
        bi.permisoEnviosEliminar = permisoEnviosEliminar
        bi.permisoEnviosModificar = permisoEnviosModificar
        bi.permisoEnviosVer = permisoEnviosVer
        bi.permisoEquivalenciasCrear = permisoEquivalenciasCrear
        bi.permisoEquivalenciasEliminar = permisoEquivalenciasEliminar
        bi.permisoEquivalenciasModificar = permisoEquivalenciasModificar
        bi.permisoEquivalenciasVer = permisoEquivalenciasVer
        bi.permisoFacturasMRDCrear = permisoFacturasMRDCrear
        bi.permisoFacturasMRDEliminar = permisoFacturasMRDEliminar
        bi.permisoFacturasMRDModificar = permisoFacturasMRDModificar
        bi.permisoFacturasMRDVer = permisoFacturasMRDVer
        bi.permisoGastosAsignarAActivo = permisoGastosAsignarAActivo
        bi.permisoGastosAsignarAProyecto = permisoGastosAsignarAProyecto
        bi.permisoGastosCrear = permisoGastosCrear
        bi.permisoGastosEliminar = permisoGastosEliminar
        bi.permisoGastosModificar = permisoGastosModificar
        bi.permisoGastosVer = permisoGastosVer
        bi.permisoIngresosCrear = permisoIngresosCrear
        bi.permisoIngresosEliminar = permisoIngresosEliminar
        bi.permisoIngresosModificar = permisoIngresosModificar
        bi.permisoIngresosVer = permisoIngresosVer
        bi.permisoInsumosCrear = permisoInsumosCrear
        bi.permisoInsumosEliminar = permisoInsumosEliminar
        bi.permisoInsumosModificar = permisoInsumosModificar
        bi.permisoInsumosVer = permisoInsumosVer
        bi.permisoInventarioCrear = permisoInventarioCrear
        bi.permisoInventarioEliminar = permisoInventarioEliminar
        bi.permisoInventarioModificar = permisoInventarioModificar
        bi.permisoInventarioVer = permisoInventarioVer
        bi.permisoLogsEliminar = permisoLogsEliminar
        bi.permisoLogsVer = permisoLogsVer
        bi.permisoModelosCrear = permisoModelosCrear
        bi.permisoModelosEliminar = permisoModelosEliminar
        bi.permisoModelosModificar = permisoModelosModificar
        bi.permisoModelosPlanos = permisoModelosPlanos
        bi.permisoModelosVer = permisoModelosVer
        bi.permisoOrdenesCrear = permisoOrdenesCrear
        bi.permisoOrdenesEliminar = permisoOrdenesEliminar
        bi.permisoOrdenesModificar = permisoOrdenesModificar
        bi.permisoOrdenesVer = permisoOrdenesVer
        bi.permisoPersonasCrear = permisoPersonasCrear
        bi.permisoPersonasEliminar = permisoPersonasEliminar
        bi.permisoPersonasModificar = permisoPersonasModificar
        bi.permisoPersonasVer = permisoPersonasVer
        bi.permisoProveedoresCrear = permisoProveedoresCrear
        bi.permisoProveedoresEliminar = permisoProveedoresEliminar
        bi.permisoProveedoresModificar = permisoProveedoresModificar
        bi.permisoProveedoresVer = permisoProveedoresVer
        bi.permisoProyectosCrear = permisoProyectosCrear
        bi.permisoProyectosEliminar = permisoProyectosEliminar
        bi.permisoProyectosModificar = permisoProyectosModificar
        bi.permisoProyectosPlanos = permisoProyectosPlanos
        bi.permisoProyectosVer = permisoProyectosVer
        bi.permisoResumenDeTarjetasCrear = permisoResumenDeTarjetasCrear
        bi.permisoResumenDeTarjetasEliminar = permisoResumenDeTarjetasEliminar
        bi.permisoResumenDeTarjetasModificar = permisoResumenDeTarjetasModificar
        bi.permisoResumenDeTarjetasVer = permisoResumenDeTarjetasVer
        bi.permisoValesDeGasolinaCrear = permisoValesDeGasolinaCrear
        bi.permisoValesDeGasolinaEliminar = permisoValesDeGasolinaEliminar
        bi.permisoValesDeGasolinaModificar = permisoValesDeGasolinaModificar
        bi.permisoValesDeGasolinaVer = permisoValesDeGasolinaVer

        bi.IsEdit = IsEdit
        bi.IsBase = IsBase
        bi.IsModel = IsModel
        bi.IsHistoric = IsHistoric

        bi.iprojectid = iprojectid
        bi.icardid = icardid

        bi.IsEdit = False

        bi.ShowDialog(Me)

        If bi.DialogResult = Windows.Forms.DialogResult.OK Then

            Dim dsBusquedaInsumosRepetidos As DataSet

            If IsBase = True Then

                dsBusquedaInsumosRepetidos = getSQLQueryAsDataset(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs WHERE ibaseid = " & baseid & " AND icardid = " & icardid & " AND icompoundinputid = " & bi.iinputid)

            Else

                If IsModel = True Then

                    dsBusquedaInsumosRepetidos = getSQLQueryAsDataset(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs WHERE imodelid = " & iprojectid & " AND icardid = " & icardid & " AND icompoundinputid = " & bi.iinputid)

                Else

                    dsBusquedaInsumosRepetidos = getSQLQueryAsDataset(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs WHERE iprojectid = " & iprojectid & " AND icardid = " & icardid & " AND icompoundinputid = " & bi.iinputid)

                End If

            End If


            If dsBusquedaInsumosRepetidos.Tables(0).Rows.Count > 0 Then

                MsgBox("Ya tienes ese Insumo insertado en este Insumo Compuesto. ¿Podrías buscarlo en la lista y cambiar la cantidad si así lo deseas?", MsgBoxStyle.OkOnly, "Insumo Repetido (Err 83)")
                dgvInsumos.EndEdit()
                Exit Sub

            End If

            Dim cantidaddeinsumo As Double = 1

            If IsBase = True Then

                Dim queries(2) As String

                queries(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompoundInput" & iinputid & " VALUES (" & iinputid & ", " & bi.iinputid & ", " & fecha & ", '" & hora & "', '" & susername & "')"
                queries(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs SELECT " & baseid & ", " & icardid & ", " & bi.iinputid & ", sinputunit, 1, " & fecha & ", '" & hora & "', '" & susername & "' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " WHERE iinputid = " & iinputid

                executeTransactedSQLCommand(0, queries)

            Else

                If IsModel = True Then

                    Dim queries(2) As String

                    queries(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompoundInput" & iinputid & " VALUES (" & iinputid & ", " & bi.iinputid & ", " & fecha & ", '" & hora & "', '" & susername & "')"
                    queries(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs SELECT " & iprojectid & ", " & icardid & ", " & bi.iinputid & ", sinputunit, 1, " & fecha & ", '" & hora & "', '" & susername & "' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " WHERE iinputid = " & iinputid

                    executeTransactedSQLCommand(0, queries)

                Else

                    Dim queries(2) As String

                    queries(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompoundInput" & iinputid & " VALUES (" & iinputid & ", " & bi.iinputid & ", " & fecha & ", '" & hora & "', '" & susername & "')"
                    queries(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs SELECT " & iprojectid & ", " & icardid & ", " & bi.iinputid & ", sinputunit, 1, " & fecha & ", '" & hora & "', '" & susername & "' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " WHERE iinputid = " & iinputid

                    executeTransactedSQLCommand(0, queries)

                End If

            End If

            Dim querySumaInsumoCompuesto As String
            Dim sumaInsumoCompuesto As Double

            If IsBase = True Then

                querySumaInsumoCompuesto = "" & _
                "SELECT SUM(btfci.dcompoundinputqty*bp.dinputfinalprice) AS precio " & _
                "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs btfci " & _
                "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompoundInput" & iinputid & " ci ON ci.icompoundinputid = btfci.icompoundinputid " & _
                "LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " i ON i.iinputid = btfci.icompoundinputid " & _
                "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfci.ibaseid = bp.ibaseid AND btfci.icompoundinputid = bp.iinputid " & _
                "WHERE btfci.ibaseid = " & iprojectid & " and btfci.icardid = " & icardid & " " & _
                "GROUP BY ci.iinputid "

            Else

                If IsModel = True Then

                    querySumaInsumoCompuesto = "" & _
                    "SELECT SUM(mtfci.dcompoundinputqty*mp.dinputfinalprice) AS precio " & _
                    "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs mtfci " & _
                    "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompoundInput" & iinputid & " ci ON ci.icompoundinputid = mtfci.icompoundinputid " & _
                    "LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " i ON i.iinputid = mtfci.icompoundinputid " & _
                    "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) mp GROUP BY iinputid, imodelid) mp ON mtfci.imodelid = mp.imodelid AND mtfci.icompoundinputid = mp.iinputid " & _
                    "WHERE mtfci.imodelid = " & iprojectid & " and mtfci.icardid = " & icardid & " " & _
                    "GROUP BY ci.iinputid "

                Else

                    querySumaInsumoCompuesto = "" & _
                    "SELECT SUM(ptfci.dcompoundinputqty*pp.dinputfinalprice) AS precio " & _
                    "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs ptfci " & _
                    "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompoundInput" & iinputid & " ci ON ci.icompoundinputid = ptfci.icompoundinputid " & _
                    "LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " i ON i.iinputid = ptfci.icompoundinputid " & _
                    "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, iprojectid) pp ON ptfci.iprojectid = pp.iprojectid AND ptfci.icompoundinputid = pp.iinputid " & _
                    "WHERE ptfci.iprojectid = " & iprojectid & " and ptfci.icardid = " & icardid & " " & _
                    "GROUP BY ci.iinputid "

                End If

            End If

            sumaInsumoCompuesto = getSQLQueryAsDouble(0, querySumaInsumoCompuesto)

            txtCostoParaTabulador.Text = FormatCurrency(sumaInsumoCompuesto, 2, TriState.True, TriState.False, TriState.True)

        End If

        Dim queryInsumos As String

        If IsBase = True Then

            queryInsumos = "" & _
            "SELECT btfci.icompoundinputid, i.sinputdescription AS 'Insumo', btfci.scompoundinputunit AS 'Unidad', btfci.dcompoundinputqty AS 'Cantidad', " & _
            "FORMAT((btfci.dcompoundinputqty*bp.dinputfinalprice), 3) AS 'Precio' " & _
            "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs btfci " & _
            "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompoundInput" & iinputid & " ci ON ci.icompoundinputid = btfci.icompoundinputid " & _
            "LEFT JOIN inputs i ON i.iinputid = btfci.icompoundinputid " & _
            "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfci.ibaseid = bp.ibaseid AND btfci.icompoundinputid = bp.iinputid " & _
            "WHERE btfci.ibaseid = " & iprojectid & " AND btfci.icardid = " & icardid & " AND ci.iinputid = " & iinputid

        Else

            If IsModel = True Then

                queryInsumos = "" & _
                "SELECT mtfci.icompoundinputid, i.sinputdescription AS 'Insumo', mtfci.scompoundinputunit AS 'Unidad', mtfci.dcompoundinputqty AS 'Cantidad', " & _
                "FORMAT((mtfci.dcompoundinputqty*mp.dinputfinalprice), 3) AS 'Precio' " & _
                "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs mtfci " & _
                "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompoundInput" & iinputid & " ci ON ci.icompoundinputid = mtfci.icompoundinputid " & _
                "LEFT JOIN inputs i ON i.iinputid = mtfci.icompoundinputid " & _
                "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) mp GROUP BY iinputid, imodelid) mp ON mtfci.imodelid = mp.imodelid AND mtfci.icompoundinputid = mp.iinputid " & _
                "WHERE mtfci.imodelid = " & iprojectid & " AND mtfci.icardid = " & icardid & " AND ci.iinputid = " & iinputid

            Else

                queryInsumos = "" & _
                "SELECT ptfci.icompoundinputid, i.sinputdescription AS 'Insumo', ptfci.scompoundinputunit AS 'Unidad', FORMAT(ptfci.dcompoundinputqty, 3) AS 'Cantidad', " & _
                "FORMAT((ptfci.dcompoundinputqty*pp.dinputfinalprice), 3) AS 'Precio' " & _
                "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs ptfci " & _
                "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompoundInput" & iinputid & " ci ON ci.icompoundinputid = ptfci.icompoundinputid " & _
                "LEFT JOIN inputs i ON i.iinputid = ptfci.icompoundinputid " & _
                "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, iprojectid) pp ON ptfci.iprojectid = pp.iprojectid AND ptfci.icompoundinputid = pp.iinputid " & _
                "WHERE ptfci.iprojectid = " & iprojectid & " AND ptfci.icardid = " & icardid & " AND ci.iinputid = " & iinputid

            End If

        End If

        setDataGridView(dgvInsumos, queryInsumos, False)

        dgvInsumos.Columns(0).Visible = False

        dgvInsumos.Columns(0).ReadOnly = True
        dgvInsumos.Columns(2).ReadOnly = True
        dgvInsumos.Columns(4).ReadOnly = True

        isFormReadyForAction = True

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub btnEliminarInsumo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminarInsumo.Click

        If MsgBox("¿Está seguro que deseas eliminar este Insumo del Insumo Compuesto?", MsgBoxStyle.YesNo, "Confirmar Eliminación de Insumo de Insumo Compuesto (Err 39)") = MsgBoxResult.Yes Then

            Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

            Dim tmpselectedinputid As Integer = 0
            Try
                tmpselectedinputid = CInt(dgvInsumos.CurrentRow.Cells(0).Value)
            Catch ex As Exception

            End Try


            Dim baseid As Integer = 0
            baseid = getSQLQueryAsInteger(0, "SELECT ibaseid FROM base ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

            If baseid = 0 Then
                baseid = 1
            End If

            If IsBase = True Then

                executeSQLCommand(0, "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs WHERE ibaseid = " & baseid & " AND icardid = " & icardid & " AND icompoundinputid = " & tmpselectedinputid)

            Else

                If IsModel = True Then

                    executeSQLCommand(0, "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs WHERE imodelid = " & iprojectid & " AND icardid = " & icardid & " AND icompoundinputid = " & tmpselectedinputid)

                Else

                    executeSQLCommand(0, "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs WHERE iprojectid = " & iprojectid & " AND icardid = " & icardid & " AND icompoundinputid = " & tmpselectedinputid)

                End If

            End If


            Dim querySumaInsumoCompuesto As String
            Dim sumaInsumoCompuesto As Double

            If IsBase = True Then

                querySumaInsumoCompuesto = "" & _
                "SELECT SUM(btfci.dcompoundinputqty*bp.dinputfinalprice) AS precio " & _
                "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs btfci " & _
                "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompoundInput" & iinputid & " ci ON ci.icompoundinputid = btfci.icompoundinputid " & _
                "LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " i ON i.iinputid = btfci.icompoundinputid " & _
                "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfci.ibaseid = bp.ibaseid AND btfci.icompoundinputid = bp.iinputid " & _
                "WHERE btfci.ibaseid = " & iprojectid & " and btfci.icardid = " & icardid & " " & _
                "GROUP BY ci.iinputid "

            Else

                If IsModel = True Then

                    querySumaInsumoCompuesto = "" & _
                    "SELECT SUM(mtfci.dcompoundinputqty*mp.dinputfinalprice) AS precio " & _
                    "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs mtfci " & _
                    "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompoundInput" & iinputid & " ci ON ci.icompoundinputid = mtfci.icompoundinputid " & _
                    "LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " i ON i.iinputid = mtfci.icompoundinputid " & _
                    "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) mp GROUP BY iinputid, imodelid) mp ON mtfci.imodelid = mp.imodelid AND mtfci.icompoundinputid = mp.iinputid " & _
                    "WHERE mtfci.imodelid = " & iprojectid & " and mtfci.icardid = " & icardid & " " & _
                    "GROUP BY ci.iinputid "

                Else

                    querySumaInsumoCompuesto = "" & _
                    "SELECT SUM(ptfci.dcompoundinputqty*pp.dinputfinalprice) AS precio " & _
                    "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs ptfci " & _
                    "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompoundInput" & iinputid & " ci ON ci.icompoundinputid = ptfci.icompoundinputid " & _
                    "LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " i ON i.iinputid = ptfci.icompoundinputid " & _
                    "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, iprojectid) pp ON ptfci.iprojectid = pp.iprojectid AND ptfci.icompoundinputid = pp.iinputid " & _
                    "WHERE ptfci.iprojectid = " & iprojectid & " and ptfci.icardid = " & icardid & " " & _
                    "GROUP BY ci.iinputid "

                End If

            End If

            sumaInsumoCompuesto = getSQLQueryAsDouble(0, querySumaInsumoCompuesto)

            txtCostoParaTabulador.Text = FormatCurrency(sumaInsumoCompuesto, 2, TriState.True, TriState.False, TriState.True)


            Dim queryInsumos As String

            If IsBase = True Then

                queryInsumos = "" & _
                "SELECT btfci.icompoundinputid, i.sinputdescription AS 'Insumo', btfci.scompoundinputunit AS 'Unidad', btfci.dcompoundinputqty AS 'Cantidad', " & _
                "FORMAT((btfci.dcompoundinputqty*bp.dinputfinalprice), 3) AS 'Precio' " & _
                "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs btfci " & _
                "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompoundInput" & iinputid & " ci ON ci.icompoundinputid = btfci.icompoundinputid " & _
                "LEFT JOIN inputs i ON i.iinputid = btfci.icompoundinputid " & _
                "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfci.ibaseid = bp.ibaseid AND btfci.icompoundinputid = bp.iinputid " & _
                "WHERE btfci.ibaseid = " & iprojectid & " AND btfci.icardid = " & icardid & " AND ci.iinputid = " & iinputid

            Else

                If IsModel = True Then

                    queryInsumos = "" & _
                    "SELECT mtfci.icompoundinputid, i.sinputdescription AS 'Insumo', mtfci.scompoundinputunit AS 'Unidad', mtfci.dcompoundinputqty AS 'Cantidad', " & _
                    "FORMAT((mtfci.dcompoundinputqty*mp.dinputfinalprice), 3) AS 'Precio' " & _
                    "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "CardCompoundInputs mtfci " & _
                    "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompoundInput" & iinputid & " ci ON ci.icompoundinputid = mtfci.icompoundinputid " & _
                    "LEFT JOIN inputs i ON i.iinputid = mtfci.icompoundinputid " & _
                    "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) mp GROUP BY iinputid, imodelid) mp ON mtfci.imodelid = mp.imodelid AND mtfci.icompoundinputid = mp.iinputid " & _
                    "WHERE mtfci.imodelid = " & iprojectid & " AND mtfci.icardid = " & icardid & " AND ci.iinputid = " & iinputid

                Else

                    queryInsumos = "" & _
                    "SELECT ptfci.icompoundinputid, i.sinputdescription AS 'Insumo', ptfci.scompoundinputunit AS 'Unidad', FORMAT(ptfci.dcompoundinputqty, 3) AS 'Cantidad', " & _
                    "FORMAT((ptfci.dcompoundinputqty*pp.dinputfinalprice), 3) AS 'Precio' " & _
                    "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "CardCompoundInputs ptfci " & _
                    "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompoundInput" & iinputid & " ci ON ci.icompoundinputid = ptfci.icompoundinputid " & _
                    "LEFT JOIN inputs i ON i.iinputid = ptfci.icompoundinputid " & _
                    "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, iprojectid) pp ON ptfci.iprojectid = pp.iprojectid AND ptfci.icompoundinputid = pp.iinputid " & _
                    "WHERE ptfci.iprojectid = " & iprojectid & " AND ptfci.icardid = " & icardid & " AND ci.iinputid = " & iinputid

                End If

            End If

            setDataGridView(dgvInsumos, queryInsumos, False)

            dgvInsumos.Columns(0).Visible = False

            dgvInsumos.Columns(0).ReadOnly = True
            dgvInsumos.Columns(2).ReadOnly = True
            dgvInsumos.Columns(4).ReadOnly = True

            isFormReadyForAction = True

            Cursor.Current = System.Windows.Forms.Cursors.Default

        End If

    End Sub


    Private Sub btnCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelar.Click

        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()

    End Sub


    Private Sub btnGuardar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGuardar.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        If validaInsumosCompuestos(False) = False Then
            Exit Sub
        End If

        Dim fecha As Integer = 0
        Dim hora As String = "00:00:00"

        fecha = getAppDate()
        hora = getAppTime()


        Dim queriesUpdate(3) As String

        queriesUpdate(0) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " SET iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "', sinputdescription = '" & txtNombreDelInsumo.Text & "', sinputunit = '" & txtUnidadDeMedida.Text & "' WHERE iinputid = " & iinputid

        queriesUpdate(1) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types SET sinputtypedescription = '" & cmbTipoDeInsumo.SelectedItem.ToString & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE iinputid = " & iinputid

        If IsBase = True Then
            queriesUpdate(2) = "INSERT INTO logs VALUES (" & getAppDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Presupuesto Base " & iprojectid & "', 'OK')"
        Else
            If IsModel = True Then
                queriesUpdate(2) = "INSERT INTO logs VALUES (" & getAppDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Modelo " & iprojectid & " : " & getSQLQueryAsString(0, "SELECT smodelname FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & " WHERE imodelid = " & iprojectid) & "', 'OK')"
            Else
                queriesUpdate(2) = "INSERT INTO logs VALUES (" & getAppDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " (" & txtNombreDelInsumo.Text & ") en el Proyecto " & iprojectid & " : " & getSQLQueryAsString(0, "SELECT sprojectname FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & " WHERE iprojectid = " & iprojectid) & "', 'OK')"
            End If
        End If

        executeTransactedSQLCommand(0, queriesUpdate)


        Dim queriesSave(10) As String

        queriesSave(0) = "" & _
        "DELETE " & _
        "FROM inputs " & _
        "WHERE iinputid = " & iinputid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " ti WHERE inputs.iinputid = ti.iinputid) "

        queriesSave(1) = "" & _
        "DELETE " & _
        "FROM inputtypes " & _
        "WHERE iinputid = " & iinputid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types tit WHERE inputtypes.iinputid = tit.iinputid) "

        queriesSave(2) = "" & _
        "DELETE " & _
        "FROM compoundinputs " & _
        "WHERE iinputid = " & iinputid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompoundInput" & iinputid & " tci WHERE compoundinputs.iinputid = tci.iinputid AND compoundinputs.icompoundinputid = tci.icompoundinputid) "

        queriesSave(3) = "" & _
        "UPDATE inputs i JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " ti ON ti.iinputid = i.iinputid SET i.iupdatedate = ti.iupdatedate, i.supdatetime = ti.supdatetime, i.supdateusername = ti.supdateusername, i.sinputdescription = ti.sinputdescription, i.sinputunit = ti.sinputunit WHERE STR_TO_DATE(CONCAT(ti.iupdatedate, ' ', ti.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(i.iupdatedate, ' ', i.supdatetime), '%Y%c%d %T') "

        queriesSave(4) = "" & _
        "UPDATE inputtypes it JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types tit ON tit.iinputid = it.iinputid SET it.iupdatedate = tit.iupdatedate, it.supdatetime = tit.supdatetime, it.supdateusername = tit.supdateusername, it.sinputtypedescription = tit.sinputtypedescription WHERE STR_TO_DATE(CONCAT(tit.iupdatedate, ' ', tit.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(it.iupdatedate, ' ', it.supdatetime), '%Y%c%d %T') "

        queriesSave(5) = "" & _
        "UPDATE compoundinputs ci JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompoundInput" & iinputid & " tci ON tci.iinputid = ci.iinputid SET ci.iupdatedate = tci.iupdatedate, ci.supdatetime = tci.supdatetime, ci.supdateusername = tci.supdateusername, ci.icompoundinputid = tci.icompoundinputid WHERE STR_TO_DATE(CONCAT(tci.iupdatedate, ' ', tci.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(ci.iupdatedate, ' ', ci.supdatetime), '%Y%c%d %T') "

        queriesSave(6) = "" & _
        "INSERT INTO inputs " & _
        "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & " ti " & _
        "WHERE NOT EXISTS (SELECT * FROM inputs i WHERE i.iinputid = ti.iinputid AND i.iinputid = " & iinputid & ") "

        queriesSave(7) = "" & _
        "INSERT INTO inputtypes " & _
        "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Input" & iinputid & "Types tit " & _
        "WHERE NOT EXISTS (SELECT * FROM inputtypes it WHERE it.iinputid = tit.iinputid AND it.iinputid = " & iinputid & ") "

        queriesSave(8) = "" & _
        "INSERT INTO compoundinputs " & _
        "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompoundInput" & iinputid & " tci " & _
        "WHERE NOT EXISTS (SELECT * FROM compoundinputs ci WHERE ci.iinputid = tci.iinputid AND ci.icompoundinputid = tci.icompoundinputid AND ci.iinputid = " & iinputid & ") "

        queriesSave(9) = "INSERT INTO logs VALUES (" & getAppDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Insumo " & iinputid & " : " & txtNombreDelInsumo.Text & "', 'OK')"

        executeTransactedSQLCommand(0, queriesSave)


        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


End Class