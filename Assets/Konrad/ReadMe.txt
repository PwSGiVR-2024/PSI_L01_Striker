Metoda do zadawania obrażeń przez obiekt

1. Zaznacz "Is Trigger" w Box Collider obiektu.
2. W skrypcie do obiektu zadającego obrażenia (np. pułapka na niedźwiedzie) użyj:

public class NAZWA_SKRYPTU : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FirstPersonController.OnTakeDamage(ILOŚĆ_DAMAGE_ZADANEGO);
        }
    }
}

3. Po przejściu przez obiekt gracz powinien dostać obrażenia.