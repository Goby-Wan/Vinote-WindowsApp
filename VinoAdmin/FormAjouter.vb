Imports System.Text

Public Class FormAjouter
    Dim typeObjet

    Dim ArrayRecup As ArrayList

    Dim tableau
    Dim typeObjetString
    Dim tableauExposant

    Dim id As Integer

    Dim fenetreP As Form1
    Dim ComboBox As ComboBox


    Public Sub init(Type As Object, TypeOb As String, Array As ArrayList, fen As Form1)
        typeObjet = Type

        typeObjetString = TypeOb
        tableau = typeObjet.toDic()

        ArrayRecup = Array
        fenetreP = fen
    End Sub






    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Me.Close()
    End Sub

    Private Sub FormAjouter_Load(sender As Object, e As EventArgs) Handles MyBase.Load


        Dim x As Integer
        Dim y As Integer
        Dim x_tab As Integer
        Dim y_tab As Integer

        x = 50
        y = 30
        x_tab = 200
        y_tab = 50


        For Each kvp As KeyValuePair(Of String, String) In tableau
            Dim label = New Label
            label.Text = kvp.Key
            label.Size = New System.Drawing.Size(x_tab - 50, y_tab - 10)
            label.Location = New System.Drawing.Point(x, y)
            Me.Controls.Add(label)

            Me.Size = New System.Drawing.Size(500, y + 150)
            Sauvegarde.Location = New Point(100, y + 50)
            Button2.Location = New Point(300, y + 50)
            If (typeObjet IsNot "Vin" And kvp.Key IsNot "exposant") Then
                Dim input = New TextBox

                input.Name = kvp.Key
                input.Size = New System.Drawing.Size(x_tab - 50, y_tab - 10)
                input.Location = New System.Drawing.Point(x + x_tab, y)
                Me.Controls.Add(input)

                y = y + y_tab
                If (input.Name = "id") Then
                    input.Enabled = False
                End If
            Else
                ComboBox = New ComboBox
                ComboBox.Name = kvp.Key
                ComboBox.Size = New System.Drawing.Size(x_tab - 50, y_tab - 10)
                ComboBox.Location = New System.Drawing.Point(x + x_tab, y)
                Me.Controls.Add(ComboBox)

                For Each exp As String() In ArrayRecup
                    ComboBox.Items.Add(exp(0) + ":" + exp(3) + " " + exp(2))


                Next
            End If

        Next



    End Sub

    Private Sub Sauvegarde_Click(sender As Object, e As EventArgs) Handles Sauvegarde.Click
        Dim content As String
        content = "{"
        Dim premier As Boolean
        premier = True
        For Each kvp As KeyValuePair(Of String, String) In tableau
            If (kvp.Key IsNot "id") Then

                If (Not premier) Then
                    content = content + ","
                Else
                    premier = False
                End If

                If (typeObjetString = "vins") Then
                    Dim ComboSelect = Split(ComboBox.SelectedItem.ToString, ":")
                    id = Convert.ToInt64(ComboSelect(0))
                End If

                Select Case kvp.Key
                        Case "id", "annee", "alcool", "prix", "score", "cp", "timestamp", "note"
                            content = content + """" + kvp.Key + """:" + Me.Controls.Item(kvp.Key).Text + ""

                        Case "exposant"
                            content = content + """exposant"":" + id.ToString + ""

                        Case Else
                            content = content + """" + kvp.Key + """:""" + Me.Controls.Item(kvp.Key).Text + """"

                    End Select

                End If
        Next

        content = content + "}"
        Console.WriteLine(content)
        MsgBox(content)

        Dim uri As New Uri("http://192.168.2.130/vinote/web/app_dev.php/" & typeObjetString)

        Dim data = Encoding.UTF8.GetBytes(content)
        Form1.SendRequest(uri, data, "application/json", "POST")
        'Form1.btCharge_Click(sender, e)

        MsgBox("Insertion effectuée")
        fenetreP.chargement()
        fenetreP.changementListe()
        Me.Close()
    End Sub
End Class