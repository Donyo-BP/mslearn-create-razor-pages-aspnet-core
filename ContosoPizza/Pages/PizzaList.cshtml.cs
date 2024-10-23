using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

// Ces instructions importent les types Pizza et PizzaService à utiliser dans la page.
using ContosoPizza.Models;
using ContosoPizza.Services;
using Microsoft.AspNetCore.DataProtection.KeyManagement.Internal;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace ContosoPizza.Pages
{

    // créé par défaut
    // public class PizzaListModel : PageModel
    // {
    //     public void OnGet()
    //     {
    //     }
    // }

    public class PizzaListModel : PageModel
    {

        // Création d'un variable PizzaService privée en lecture seule nommée _service . Cette variable contient une référence vers un objet PizzaService.
        // Le mot clé readonly indique que la valeur de la variable _service ne peut plus être changée après qu’elle a été définie dans le constructeur.
        private readonly PizzaService _service;

        // Une propriété PizzaList est définie pour contenir la liste des pizzas.
        // Le type IList<Pizza> indique que la propriété PizzaList contiendra une liste d’objets Pizza.
        // PizzaList est initialisé sur default! pour indiquer au compilateur qu’il sera initialisé ultérieurement.De cette façon, les vérifications de sécurité Null ne sont pas requises.
        public IList<Pizza> PizzaList { get; set; } = default!;

        // Le constructeur accepte un objet PizzaService.
        // L’objet PizzaService est fourni par injection de dépendances.
        public PizzaListModel(PizzaService service)
        {
            _service = service;
        }

        // Une méthode OnGet est définie pour récupérer la liste des pizzas à partir de l’objet PizzaService et la stocker dans la propriété PizzaList.
        public void OnGet()
        {
            PizzaList = _service.GetPizzas();
        }

        [BindProperty] // L’attribut BindProperty est appliqué à la propriété NewPizza.
                       // L’attribut BindProperty lie la propriété NewPizza à la page Razor. Quand une requête HTTP POST est effectuée,
                       // la propriété NewPizza est remplie avec l’entrée de l’utilisateur.
        public Pizza NewPizza { get; set; }  = default!;
        // Une propriété nommée NewPizza est ajoutée à la classe PizzaListModel
        // NewPizza est un objet Pizza.
        // La propriété NewPizza est initialisée à default!.
        // Le mot clé default! est utilisé pour initialiser la propriété NewPizza sur null. 
        // Cela évite au compilateur de générer un avertissement à propos de la propriété NewPizza non initialisée.


        // Ajout du gestionnaire de pages pour HTTP POST. Ajout de la méthode IActionResult à la classe PizzaListModel :
        public IActionResult OnPost()
        {
            // La méthode OnPost est appelée quand une requête HTTP POST est effectuée.
            // La méthode OnPost est utilisée pour ajouter une nouvelle pizza à la liste des pizzas.


            // La propriété ModelState.IsValid détermine si l’entrée de l’utilisateur est valide.
            // Si la propriété NewPizza n’est pas null, la méthode OnPost est appelée
            if (!ModelState.IsValid || NewPizza == null)
            {
            // Les règles de validation sont déduites de certains attributs(comme Required et Range) de la classe Pizza dans Models\Pizza.cs.
            // Si l’entrée de l’utilisateur n’est pas valide, la méthode Page est appelée pour un nouveau rendu de la page.
                return Page();
            }

            // La propriété NewPizza ajoute une nouvelle pizza à l’objet _service.
            _service.AddPizza(NewPizza);

            // La méthode RedirectToAction redirige l’utilisateur vers le gestionnaire de page Get, qui recommence le rendu de la page avec la liste mise à jour des pizzas.
            return RedirectToAction("Get");

        }

        public IActionResult OnPostDelete(int id)
        {
            // La méthode OnPostDelete est appelée quand une requête HTTP POST est effectuée
            // La méthode OnPostDelete est utilisée pour supprimer une pizza de la liste des pizzas
            // La propriété id est utilisée pour identifier la pizza à supprimer.
            if (id == 0)
            {
                return Page();
            }
            // La propriété id est utilisée pour identifier la pizza à supprimer.
            _service.DeletePizza(id);
            // La méthode RedirectToAction redirige l’utilisateur vers le gestionnaire de page Get, qui
            // recommence le rendu de la page avec la liste mise à jour des pizzas.
            return RedirectToAction("Get");


            // La méthode OnPostDelete est appelée quand l’utilisateur clique sur le bouton Delete(Supprimer) pour supprimer une pizza.
            // La page sait qu’elle doit utiliser cette méthode, car l’attribut asp-page - handler sur le bouton Delete dans Pages\PizzaList.cshtml est défini sur Delete.
            // Le paramètre id identifie la pizza à supprimer.
            // Le paramètre id est lié à la valeur de route id dans l’URL. Cela s’effectue avec l’attribut asp-route-id sur le bouton Delete dans Pages\PizzaList.cshtml.
            // La méthode DeletePizza est appelée sur l’objet _service pour supprimer la pizza.
            // La méthode RedirectToAction redirige l’utilisateur vers le gestionnaire de page Get, qui recommence le rendu de la page avec la liste mise à jour des pizzas.

        }
    }


}
