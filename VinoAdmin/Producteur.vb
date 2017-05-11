

Public Class Producteur
    Public id As Integer
    Public nom As String
    Public prenom As String
    Public domaine As String
    Public Function toArrayString()
        Dim result As String() = {id.ToString(), domaine, nom, prenom}
        Return result

    End Function
End Class
