using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    public class Missile : MonoBehaviour
    {
        [SerializeField]
        private ParticleSystem _particle;
        [SerializeField]
        private GameObject _explosionPrefab; 
    

        private Rigidbody _rigidbody; 
        private AudioSource _audioSource; 
        
       
        [SerializeField] private Vector3 _target;
     
        private float elapsedTime;
    private float flightDuration = 1f;
    public float arcHeight = 2f;
    private Vector3 startPoint;
   

    void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _audioSource = GetComponent<AudioSource>(); 
            _audioSource.pitch = Random.Range(0.7f, 1.9f); 
            _particle.Play(); 
            _audioSource.Play();
        startPoint = transform.position;
    }


    void Update()
    {

        elapsedTime += Time.deltaTime;
        float t = Mathf.Clamp01(elapsedTime / flightDuration);

        if (_target != null) 
        {
            Vector3 linearPos = Vector3.Lerp(startPoint, _target, t);
            float height = arcHeight * 4 * (t - t * t);
            transform.position = linearPos + Vector3.up * height;

            if (transform.position == _target)
            {
                Explode();
            }
        }
    }


        public void AssignMissleRules(Vector3 target, float destroyTimer, float duration)
        {
            _target = new Vector3(target.x, 0, target.z); 
            flightDuration = duration;
            Destroy(this.gameObject, destroyTimer); 
        }

    private void Explode()
    {
    
        if (_explosionPrefab != null)
        {
            GameObject expl = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(expl, 1);
        }
       
        Destroy(this.gameObject);
    
    }
}


