using UnityEngine;
using UnityEngine.SceneManagement;

public class Gard_Veiw_Area : MonoBehaviour
{
    public ContactFilter2D contact;
    private Transform Player;
    private float Alert = 0;
    private SpriteRenderer sr;
    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        sr = GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        sr.color = new Color(150f / 255f, Mathf.Lerp(150f / 255f, 0, Alert), Mathf.Lerp(150f / 255f, 0, Alert), 1);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Player.position - transform.position, 10);
        if (hit && hit.transform.tag == "Player")
        {
            Alert += Time.deltaTime * 2;
        }
        else
        {
            Alert -= Time.deltaTime;
        }
        Alert = Mathf.Clamp(Alert, 0, 1);
        if (Alert >= 1f)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
