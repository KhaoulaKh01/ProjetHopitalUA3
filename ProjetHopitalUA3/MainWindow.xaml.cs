using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProjetHopitalUA3
{
    using Microsoft.Data.SqlClient;
    using System.Collections.ObjectModel;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    //DESKTOP-1AQ9LC7\SQLEXPRESS

    using System.Data;
    using System.Windows;
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            

            
            // Charger la liste des spécialités
            Charger_liste_specialite();

            // Charger la liste des médecins
            Charger_liste_medecin(); // Appel de la méthode pour charger la liste des médecins

            // Mettre à jour le ComboBox avec les spécialités au chargement de la fenêtre
            UpdateComboBoxSpecialitesMedecin();

        }

        // Méthode appelée lorsque la fenêtre est chargée
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Mettre à jour le ComboBox avec les spécialités au chargement de la fenêtre
            UpdateComboBoxSpecialitesMedecin();
        }
        
        // la chaîne de connexion
        public static string ConnectionString = "Data Source=DESKTOP-1AQ9LC7\\SQLEXPRESS;Initial Catalog=GestionHopital;Integrated Security=True;Encrypt=True;Trust Server Certificate=True;";

        SqlConnection conDB = new SqlConnection(ConnectionString);


        // Cette méthode est appelée lorsque l'utilisateur clique sur la fenêtre avec le bouton gauche de la souris et maintient le clic.
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // La méthode DragMove() permet de déplacer la fenêtre en maintenant le bouton de la souris enfoncé et en la déplaçant.
            this.DragMove();
        }

        // Cette méthode pour le bouton de fermeture.
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            // La méthode Close() ferme la fenêtre actuelle.
            this.Close();
        }

        // Afficher le MenuItem "Médecin" dans le menu
        private void MenuItemMedecin_Click(object sender, RoutedEventArgs e)

        {

            // Afficher la page correspondante dans le TabControl

            if (tabControlPrincipal != null)

            {

                tabControlPrincipal.SelectedIndex = 1;

            }

        }
        // Afficher le MenuItem "Spécialité" dans le menu
        private void MenuItemSpecialite_Click(object sender, RoutedEventArgs e)

        {

            // Afficher la page correspondante dans le TabControl

            if (tabControlPrincipal != null)

            {

                tabControlPrincipal.SelectedIndex = 0;

            }

        }
        // Afficher le MenuItem "Consultation" dans le menu
        private void MenuItemConsultation_Click(object sender, RoutedEventArgs e)

        {

            // Afficher la page correspondante dans le TabControl

            if (tabControlPrincipal != null)

            {

                tabControlPrincipal.SelectedIndex = 2;

            }

        }

        //Méthode exécutée lorsqu'on clique sur le bouton de réinitialisation.
        private void btnReset_Click(object sender, RoutedEventArgs e)

        {

            // Effacez le contenu des champs de medecins

            tbIdentifiantMedecin.Text = "";

            tbNomMedecin.Text = "";

            tbPrenomMedecin.Text = "";

            cmbSpecialiteMedecin.SelectedItem = null;

            tbTelephoneMedecin.Text = "";

            tbSalaireMedecin.Text = "";

            // Effacez le contenu des champs de specialite

            tbIdentifiantSpecialite.Text = "";

            tbNomSpecialite.Text = "";

            tbDescription.Text = "";

        }
        //*****************Spécialité
        //métohe d'ajout d'une spécialité
        
         private void btnAjouterSpecialite_Click(object sender, RoutedEventArgs e)

        {

            try

            {

                // Vérifier que les champs requis ne sont pas vides

                if (string.IsNullOrWhiteSpace(tbNomSpecialite.Text))

                {

                    throw new Exception("Le champ 'Nom' est requis.");

                }

                if (string.IsNullOrWhiteSpace(tbDescription.Text))

                {

                    throw new Exception("Le champ 'Description' est requis.");

                }

                // Ouvrir la connexion si elle n'est pas déjà ouverte

                if (conDB.State != ConnectionState.Open)

                {

                    conDB.Open();

                }

                // Requête SQL pour insérer une spécialité

                string requete = "INSERT INTO Specialite (Nom, Description) VALUES(@NomSpecialite, @Description)";

                // Créer une commande SQL

                SqlCommand cmd = new SqlCommand(requete, conDB);

                cmd.CommandType = CommandType.Text;

                // Ajouter les paramètres

                cmd.Parameters.AddWithValue("@NomSpecialite", tbNomSpecialite.Text);

                cmd.Parameters.AddWithValue("@Description", tbDescription.Text);

                // Exécuter la commande

                cmd.ExecuteNonQuery();

                // Actualiser la liste des spécialités

                Charger_liste_specialite();

                UpdateComboBoxSpecialitesMedecin();

                // Rafraîchir la DataGrid

                dgSpecialite.Items.Refresh();

                // Rafraîchir le ComboBox cmbSpecialiteMedecin

                cmbSpecialiteMedecin.Items.Refresh();

                cbMedecinSpecialite.Items.Refresh();

                // Afficher un message de succès

                statusMessage.Text = "Spécialité ajoutée avec succès!";

                statusAction.Foreground = Brushes.Green;

                statusAction.Text = "OK";

                // Réinitialiser les champs après l'ajout

                tbIdentifiantSpecialite.Text = "";

                tbNomSpecialite.Text = "";

                tbDescription.Text = "";

            }

            catch (Exception ex)

            {

                // Afficher un message d'erreur en cas d'échec

                statusMessage.Text = ex.Message;

                statusAction.Text = "erreur";

                statusAction.Foreground = Brushes.Red;

            }

        }


        //methode qui met à jour le combobox de liste spécialité 
        private void UpdateComboBoxSpecialitesMedecin()
        {
            try
            {
                // Récupérer la liste des noms de spécialités de la DataGrid
                List<string> specialites = new List<string>();

                // Assurez-vous que la DataGrid a des éléments
                if (dgSpecialite.Items.Count > 0)
                {
                    // Supposons que la colonne contenant les noms de spécialités s'appelle "Nom"
                    foreach (DataRowView item in dgSpecialite.Items)
                    {
                        specialites.Add(item["Nom"].ToString());
                    }
                }

                // Mettre à jour la source de données du ComboBox cmbSpecialiteMedecin et cbMedecinSpecialite
                cmbSpecialiteMedecin.ItemsSource = specialites;
                cbMedecinSpecialite.ItemsSource = specialites;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la mise à jour du ComboBox des spécialités : " + ex.Message);
            }
        }




        private void Charger_liste_specialite()
        {
            try
            {
                // Requête SQL pour récupérer toutes les données de la table Specialite
                string requete = "SELECT * FROM Specialite";

                // Requête SQL pour récupérer uniquement les noms des spécialités
                string requete0 = "SELECT Nom FROM Specialite";

                // Vérifier si la connexion à la base de données est ouverte, et l'ouvrir si elle ne l'est pas
                if (conDB.State != ConnectionState.Open)
                {
                    conDB.Open();
                }
                // Créer une commande SQL pour exécuter la première requête
                SqlCommand cmd = new SqlCommand(requete, conDB);
                // Exécuter la commande et récupérer les résultats dans un DataReader
                SqlDataReader sqlDataReader = cmd.ExecuteReader();
                // Créer un DataTable et charger les données du DataReader
                DataTable dt = new DataTable();
                dt.Load(sqlDataReader);

                // Afficher les données dans le DataGrid
                dgSpecialite.ItemsSource = dt.DefaultView;

                // Fermer le DataReader
                sqlDataReader.Close();

                // Recharger la liste des spécialités après la fermeture du DataReader
                SqlCommand cmd0 = new SqlCommand(requete0, conDB);
                SqlDataReader reader = cmd0.ExecuteReader();

                // Créer une liste pour stocker les noms des spécialités
                List<string> specialites = new List<string>();

                // Lire chaque enregistrement et ajouter le nom de la spécialité à la liste
                while (reader.Read())
                {
                    specialites.Add(reader["Nom"].ToString());
                }

                // Fermer le DataReader
                reader.Close();

                // Mettre à jour la source de données des ComboBox
                cmbSpecialiteMedecin.ItemsSource = specialites;
                cbMedecinSpecialite.ItemsSource = specialites;
            }
            catch (Exception ex)
            {
                // Afficher un message d'erreur en cas d'échec lors du chargement des spécialités
                MessageBox.Show("Erreur lors du chargement des spécialités : " + ex.Message);
            }
            finally
            {
                // Fermer la connexion à la base de données après l'opération
                if (conDB.State == ConnectionState.Open)
                {
                    conDB.Close();
                }
            }
        }



        private void btnSupprimerSpecialite_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Vérifier si une spécialité est sélectionnée dans le DataGrid
                if (dgSpecialite.SelectedItem != null)
                {
                    DataRowView row = (DataRowView)dgSpecialite.SelectedItem;
                    int identifiant = Convert.ToInt32(row["Identifiant"]);

                    // Vérifier s'il y a des médecins associés à cette spécialité
                    string queryCheck = "SELECT COUNT(*) FROM Medecin WHERE Specialite = @Specialite";
                    using (SqlCommand cmdCheck = new SqlCommand(queryCheck, conDB))
                    {
                        cmdCheck.Parameters.AddWithValue("@Specialite", row["Nom"]);
                        // Ouvrir la connexion à la base de données
                        conDB.Open();
                        // Exécuter la requête pour compter le nombre de médecins associés à la spécialité
                        int count = Convert.ToInt32(cmdCheck.ExecuteScalar());
                        // Fermer la connexion après l'exécution de la requête
                        conDB.Close();
                        // Vérifier s'il y a des médecins associés
                        if (count > 0)
                        {
                            // Afficher un message d'erreur si des médecins sont associés à cette spécialité
                            statusMessage.Text = "Cette spécialité est associée à des médecins. Impossible de la supprimer.";
                            return;
                        }
                    }

                    // Supprimer la spécialité si aucun médecin n'est associé
                    if (conDB.State != ConnectionState.Open)
                    {
                        // Ouvrir la connexion à la base de données
                        conDB.Open();
                    }
                    
                    string requete = "DELETE FROM Specialite WHERE Identifiant = @IdentifiantSpecialite";
                    SqlCommand cmd = new SqlCommand(requete, conDB);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@IdentifiantSpecialite", identifiant);
                    // Exécuter la commande de suppression
                    int rowsAffected = cmd.ExecuteNonQuery();
                    // Fermer la connexion après la suppression
                    conDB.Close();
                    // Vérifier si la suppression a réussi
                    if (rowsAffected > 0)
                    {
                        // Mettre à jour la liste des spécialités affichées dans le DataGrid

                        Charger_liste_specialite(); // Mettre à jour la liste des spécialités
                        // Afficher un message de succès
                        statusMessage.Text = "Spécialité supprimée!";
                        statusAction.Foreground = Brushes.Green;
                        statusAction.Text = "OK";
                    }
                    else
                    {
                        // Afficher un message si la suppression a échoué
                        statusMessage.Text = "La spécialité n'a pas pu être supprimée.";

                    }
                }
                else
                {
                    // Afficher un message si aucune spécialité n'est sélectionnée
                    statusMessage.Text = "Veuillez sélectionner une spécialité à supprimer!";
                }
            }
            catch (Exception ex)
            {
                // Afficher un message d'erreur en cas d'exception
                statusMessage.Text = "Erreur : " + ex.Message;
                statusAction.Text = "erreur";
                statusAction.Foreground = Brushes.Red;
            }
        }





        private void btnModifierSpecialite_Click(object sender, RoutedEventArgs e)
        {
            try

            {
                // Ouvrir la connexion si elle n'est pas déjà ouverte

                if (conDB.State != ConnectionState.Open)

                {

                    conDB.Open();

                }

                // Requête SQL pour modifier une spécialité

                string requete = "UPDATE Specialite SET Nom = @NomSpecialite, Description = @Description WHERE Identifiant = @IdentifiantSpecialite";

                // Créer une commande SQL

                SqlCommand cmd = new SqlCommand(requete, conDB);

                cmd.CommandType = CommandType.Text;

                // Ajouter les paramètres

                cmd.Parameters.AddWithValue("@IdentifiantSpecialite", tbIdentifiantSpecialite.Text);

                cmd.Parameters.AddWithValue("@NomSpecialite", tbNomSpecialite.Text);

                cmd.Parameters.AddWithValue("@Description", tbDescription.Text);

                // Exécuter la commande

                cmd.ExecuteNonQuery();

                // Actualiser la liste des spécialités

                Charger_liste_specialite();

                UpdateComboBoxSpecialitesMedecin();

                // Afficher un message de succès

                statusMessage.Text = "Spécialité modifiée avec succès!";
                statusAction.Foreground = Brushes.Green;
                statusAction.Text = "OK";

                // Réinitialiser les champs après la modification

                tbIdentifiantSpecialite.Clear();

                tbNomSpecialite.Clear();

                tbDescription.Clear();

            }

            catch (Exception ex)

            {

                // Afficher un message d'erreur en cas d'échec

                statusMessage.Text = ex.Message;
                statusAction.Text = "erreur";
                statusAction.Foreground = Brushes.Red;


            }

        }



        //****************Médecin
       
        // Méthode pour gérer l'ajout d'un nouveau médecin
      
        private void btnAjouterMedecin_Click(object sender, RoutedEventArgs e)

        {

            try

            {

                // Vérifier que tous les champs requis sont renseignés

                if (string.IsNullOrWhiteSpace(tbNomMedecin.Text))

                {

                    throw new Exception("Le champ 'Nom' est requis.");

                }

                if (string.IsNullOrWhiteSpace(tbPrenomMedecin.Text))

                {

                    throw new Exception("Le champ 'Prénom' est requis.");

                }

                if (cmbSpecialiteMedecin.SelectedItem == null)

                {

                    throw new Exception("Veuillez sélectionner une spécialité.");

                }

                if (string.IsNullOrWhiteSpace(tbTelephoneMedecin.Text))

                {

                    throw new Exception("Le champ 'Téléphone' est requis.");

                }

                if (string.IsNullOrWhiteSpace(tbSalaireMedecin.Text))

                {

                    throw new Exception("Le champ 'Salaire' est requis.");

                }

                // Ouvrir la connexion si elle n'est pas déjà ouverte

                if (conDB.State != ConnectionState.Open)

                {

                    conDB.Open();

                }

                // Requête SQL pour ajouter un nouveau médecin

                string requete = "INSERT INTO Medecin (Nom, Prenom, Specialite, Telephone, Salaire) " +

                                 "VALUES (@NomMedecin, @PrenomMedecin, @SpecialiteMedecin, @TelephoneMedecin, @SalaireMedecin)";

                // Créer une commande SQL

                SqlCommand cmd = new SqlCommand(requete, conDB);

                cmd.CommandType = CommandType.Text;

                // Ajouter les paramètres

                cmd.Parameters.AddWithValue("@NomMedecin", tbNomMedecin.Text);

                cmd.Parameters.AddWithValue("@PrenomMedecin", tbPrenomMedecin.Text);

                cmd.Parameters.AddWithValue("@SpecialiteMedecin", cmbSpecialiteMedecin.SelectedItem.ToString());

                cmd.Parameters.AddWithValue("@TelephoneMedecin", tbTelephoneMedecin.Text);

                // Convertir le salaire en décimal

                decimal salaire;

                if (!decimal.TryParse(tbSalaireMedecin.Text.Replace('.', ','), out salaire))

                {

                    throw new Exception("Salaire invalide. Veuillez saisir un nombre valide.");

                }

                cmd.Parameters.AddWithValue("@SalaireMedecin", salaire);

                // Exécuter la commande

                int rowsAffected = cmd.ExecuteNonQuery();

                // Si au moins une ligne a été affectée, actualiser la liste des médecins

                if (rowsAffected > 0)

                {

                    // Mettre à jour la liste des médecins dans le DataGrid

                    Charger_liste_medecin();

                    // Afficher un message de succès

                    statusMessage.Text = "Nouveau médecin ajouté!";

                    statusAction.Foreground = Brushes.Green;

                    statusAction.Text = "OK";

                }

                else

                {

                    statusMessage.Text = "Aucun médecin ajouté.";

                }

            }

            catch (Exception ex)

            {

                // Afficher un message d'erreur en cas d'échec

                statusMessage.Text = ex.Message;

                statusAction.Text = "erreur";

                statusAction.Foreground = Brushes.Red;

            }

            finally

            {

                // Fermer la connexion après l'opération

                if (conDB.State == ConnectionState.Open)

                {

                    conDB.Close();

                }

            }

        }



        // Méthode pour charger la liste des médecins
        private void Charger_liste_medecin()
        {
            try
            {
                // Requête SQL pour sélectionner tous les médecins
                string requete = "SELECT * FROM Medecin";
                // Ouvrir la connexion si elle n'est pas déjà ouverte
                if (conDB.State != ConnectionState.Open)
                {
                    conDB.Open();
                }
                // Créer une commande SQL avec la requête
                SqlCommand cmd = new SqlCommand(requete, conDB);
                // Exécuter la commande et récupérer les résultats dans un SqlDataReader
                SqlDataReader sqlDataReader = cmd.ExecuteReader();
                // Créer un DataTable et charger les données du SqlDataReader
                DataTable dt = new DataTable();
                dt.Load(sqlDataReader);

                // Affecter la source de données du DataGrid avec le DataTable chargé
                dgListeMedecins.ItemsSource = dt.DefaultView;
                dgMedecin.ItemsSource = dt.DefaultView;
                // Afficher un message de succès
                statusMessage.Text = "Chargement de la liste des médecins réussi.";
            }
            catch (Exception ex)
            {
                // Afficher un message d'erreur en cas d'échec
                statusMessage.Text = "Erreur lors du chargement des médecins : " + ex.Message;
            }
            finally
            {
                // Fermer la connexion après l'opération
                if (conDB.State == ConnectionState.Open)
                {
                    conDB.Close();
                }
            }
        }





        // Méthode de suppression d'un médecin

        
        private void btnSupprimerMedecin_Click(object sender, RoutedEventArgs e)

        {

            try

            {

                if (dgMedecin.SelectedItem != null)

                {

                    // Récupérer la ligne sélectionnée dans le DataGrid

                    DataRowView row = (DataRowView)dgMedecin.SelectedItem;

                    int identifiant = Convert.ToInt32(row["Identifiant"]);

                    // Vérifier si l'identifiant du médecin est correct

                    Console.WriteLine("Identifiant du médecin sélectionné : " + identifiant);

                    if (conDB.State != ConnectionState.Open)

                    {

                        conDB.Open();

                    }

                    // Requête SQL pour supprimer le médecin

                    string requete = "DELETE FROM Medecin WHERE Identifiant = @IdentifiantMedecin";

                    SqlCommand cmd = new SqlCommand(requete, conDB);

                    cmd.CommandType = CommandType.Text;

                    // Ajouter le paramètre de l'identifiant du médecin

                    cmd.Parameters.AddWithValue("@IdentifiantMedecin", identifiant);

                    // Exécuter la commande de suppression

                    int rowsAffected = cmd.ExecuteNonQuery();

                    conDB.Close();

                    // Vérifier si la suppression a réussi

                    Console.WriteLine("Nombre de lignes supprimées : " + rowsAffected);

                    if (rowsAffected > 0)

                    {

                        // Mettre à jour le DataGrid des médecins

                        Charger_liste_medecin();

                        statusMessage.Text = "Médecin supprimé!";

                        statusAction.Foreground = Brushes.Green;

                        statusAction.Text = "OK";

                    }

                    else

                    {

                        statusMessage.Text = "Le médecin n'a pas pu être supprimé.";

                    }

                }

                else

                {

                    statusMessage.Text = "Veuillez sélectionner un médecin à supprimer!";

                }

            }

            catch (Exception ex)

            {

                // Afficher les détails de l'erreur

                Console.WriteLine("Erreur : " + ex.Message);

                statusMessage.Text = "Erreur : " + ex.Message;

                statusAction.Text = "erreur";

                statusAction.Foreground = Brushes.Red;

            }

        }


        //permet de modifier un médecin

        //veuillez modifier le salaire
        private void btnModifierMedecin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Vérifier si une ligne est sélectionnée dans le DataGrid des médecins
                if (dgMedecin.SelectedItem != null)
                {
                    // Récupérer la ligne sélectionnée
                    DataRowView row = (DataRowView)dgMedecin.SelectedItem;

                    // Récupérer l'identifiant du médecin
                    int identifiant = Convert.ToInt32(row["Identifiant"]);

                    // Ouvrir la connexion si elle n'est pas déjà ouverte
                    if (conDB.State != ConnectionState.Open)
                    {
                        conDB.Open();
                    }

                    // Requête SQL pour modifier un médecin
                    string requete = "UPDATE Medecin SET Nom = @NomMedecin, Prenom = @PrenomMedecin, Specialite = @SpecialiteMedecin, Telephone = @TelephoneMedecin, Salaire = @SalaireMedecin WHERE Identifiant = @IdentifiantMedecin";

                    // Créer une commande SQL
                    SqlCommand cmd = new SqlCommand(requete, conDB);
                    cmd.CommandType = CommandType.Text;

                    // Ajouter les paramètres
                    cmd.Parameters.AddWithValue("@IdentifiantMedecin", tbIdentifiantMedecin.Text);
                    cmd.Parameters.AddWithValue("@NomMedecin", tbNomMedecin.Text);
                    cmd.Parameters.AddWithValue("@PrenomMedecin", tbPrenomMedecin.Text);
                    cmd.Parameters.AddWithValue("@SpecialiteMedecin", cmbSpecialiteMedecin.SelectedValue); // Utilisation de la valeur sélectionnée dans la ComboBox
                    cmd.Parameters.AddWithValue("@TelephoneMedecin", tbTelephoneMedecin.Text);
                    cmd.Parameters.AddWithValue("@SalaireMedecin", tbSalaireMedecin.Text);

                    // Exécuter la commande
                    cmd.ExecuteNonQuery();

                    // Actualiser le DataGrid des médecins
                    Charger_liste_medecin();

                    // Afficher un message de succès
                    statusMessage.Text = "Médecin modifié avec succès!";

                    // Réinitialiser les champs après la modification
                    tbIdentifiantMedecin.Clear();
                    tbNomMedecin.Clear();
                    tbPrenomMedecin.Clear();
                    cmbSpecialiteMedecin.SelectedIndex = -1; // Effacer la sélection de la ComboBox
                    tbTelephoneMedecin.Clear();
                    tbSalaireMedecin.Clear();
                }
                else
                {
                    // Afficher un message si aucun médecin n'est sélectionné
                    statusMessage.Text = "Veuillez sélectionner un médecin à modifier!";
                }
            }
            catch (Exception ex)
            {
                // Afficher un message d'erreur en cas d'échec
                statusMessage.Text = "Erreur : " + ex.Message;
            }
        }



        //Cette méthode est appelée lorsque la sélection dans le DataGrid des médecins change.
        private void dgMedecin_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Vérifier si une ligne est sélectionnée
            if (dgMedecin.SelectedItem != null)
            {
                // Récupérer la ligne sélectionnée
                DataRowView row = (DataRowView)dgMedecin.SelectedItem;

                // Extraire les valeurs de la ligne pour l'identifiant, le nom, le prénom, la spécialité, le téléphone et le salaire
                int identifiant = Convert.ToInt32(row["Identifiant"]);
                string nom = row["Nom"].ToString();
                string prenom = row["Prenom"].ToString();
                string specialite = row["Specialite"].ToString();
                string telephone = row["Telephone"].ToString();
                decimal salaire = Convert.ToDecimal(row["Salaire"]);

                // Mettre à jour les champs dans l'interface utilisateur avec les valeurs extraites
                tbIdentifiantMedecin.Text = identifiant.ToString();
                tbNomMedecin.Text = nom;
                tbPrenomMedecin.Text = prenom;
                tbTelephoneMedecin.Text = telephone;
                tbSalaireMedecin.Text = salaire.ToString();

                // Sélectionner la spécialité dans la ComboBox
                foreach (var item in cmbSpecialiteMedecin.Items)
                {
                    if (item.ToString() == specialite)
                    {
                        cmbSpecialiteMedecin.SelectedItem = item;
                        break;
                    }
                }
            }
        }


        //Cette méthode est appelée lorsque la sélection dans le DataGrid des spécialités change
        private void dgSpecialite_SelectionChanged(object sender, SelectionChangedEventArgs e)

        {

            // Vérifier si une ligne est sélectionnée

            if (dgSpecialite.SelectedItem != null)

            {

                // Récupérer la ligne sélectionnée

                DataRowView row = (DataRowView)dgSpecialite.SelectedItem;

                // Extraire les valeurs de la ligne pour l'identifiant, le nom et la description

                int identifiant = Convert.ToInt32(row["Identifiant"]);

                string nom = row["Nom"].ToString();

                string description = row["Description"].ToString();

                // Mettre à jour les champs dans l'interface utilisateur avec les valeurs extraites

                tbIdentifiantSpecialite.Text = identifiant.ToString();

                tbNomSpecialite.Text = nom;

                tbDescription.Text = description;

            }

        }

        //Permet de rechercher les médecins d'apres le choix de l'utilisateur :salaire et spécialité
        private void btnRecherche_Click(object sender, RoutedEventArgs e)
        {
            try
            {
            
                string specialite = cbMedecinSpecialite.Text;
                decimal salaire;
                string comparateur = "";

                // Vérifier si une spécialité est sélectionnée
                if (string.IsNullOrEmpty(specialite))
                {
                    throw new Exception("Veuillez sélectionner une spécialité.");
                }

                // Vérifier si le salaire est un nombre valide
                if (!decimal.TryParse(tbSalaireConsultation.Text, out salaire))
                {
                    throw new Exception("Veuillez saisir un salaire valide.");
                }

                // Vérifier le critère de comparaison du salaire
                if (chkSalaireEgal.IsChecked == true)
                {
                    comparateur = "=";
                }
                else if (chkSalaireInferieur.IsChecked == true)
                {
                    comparateur = "<";
                }
                else if (chkSalaireSuperieur.IsChecked == true)
                {
                    comparateur = ">";
                }
                else
                {
                    throw new Exception("Veuillez sélectionner un critère de comparaison pour le salaire.");
                }

                // Construire la requête SQL en fonction des critères sélectionnés
                string requete = "SELECT Identifiant, Nom, Prenom, Specialite, Telephone, Salaire FROM Medecin " +
                                 "WHERE Specialite = @Specialite AND Salaire " + comparateur + " @Salaire";

                // Ouvrir la connexion à la base de données
                if (conDB.State != ConnectionState.Open)
                {
                    conDB.Open();
                }

                // Créer une commande SQL avec la requête
                SqlCommand cmd = new SqlCommand(requete, conDB);
                cmd.Parameters.AddWithValue("@Specialite", specialite);
                cmd.Parameters.AddWithValue("@Salaire", salaire);

                // Exécuter la commande et récupérer les résultats dans un DataTable
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                // Vérifier si des résultats sont renvoyés
                if (dt.Rows.Count > 0)
                {
                    // Afficher les résultats dans le DataGrid
                    dgListeMedecins.ItemsSource = dt.DefaultView;
                    statusMessage.Text = "Liste des médecins consultée avec succès.";
                    statusAction.Foreground = Brushes.Green;

                    statusAction.Text = "OK";
                }
                else
                {
                    // Aucun résultat trouvé, afficher un message approprié
                    statusMessage.Text = "Aucun médecin trouvé pour ces critères de recherche.";
                    // Effacer le contenu du DataGrid
                    dgListeMedecins.ItemsSource = null;
                }
            }
            catch (Exception ex)
            {
                // Afficher les erreurs rencontrées
                statusMessage.Text = "Erreur lors de la consultation des médecins : " + ex.Message;
                statusAction.Text = "erreur";

                statusAction.Foreground = Brushes.Red;
            }
            finally
            {
                // Fermer la connexion à la base de données après l'opération
                if (conDB.State == ConnectionState.Open)
                {
                    conDB.Close();
                }
            }
        }




        //permet de sélectionner une seule checkBox
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            // Obtenez la case à cocher qui a été cochée
            CheckBox checkedBox = sender as CheckBox;

            // Désactivez toutes les autres cases
            foreach (CheckBox checkBox in new CheckBox[] { chkSalaireEgal, chkSalaireInferieur, chkSalaireSuperieur })
            {
                if (checkBox != checkedBox)
                {
                    checkBox.IsChecked = false;
                }
            }
        }




        // Ajouter une nouvelle classe pour gerer les médecins.

        public class Medecin

        {

            public int Identifiant { get; set; }

            public string Nom { get; set; }

            public string Prenom { get; set; }

            public string Specialite { get; set; }

            public long Telephone { get; set; }

            public double Salaire { get; set; }

        }

        public class Specialite

        {

            public int IdentifiantSpecialite { get; set; }

            public string NomSpecialite { get; set; }

            public string Description { get; set; }

        }



    }
}