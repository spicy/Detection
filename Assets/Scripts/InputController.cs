using System;
using System.Collections;
using System.Threading;
using UnityEngine;

namespace Detection
{
    public class InputController : MonoBehaviour
    {
        [Header("Settings")]
        public Camera cam;

        private Vector2 aimPos;
        private LookScanner mainScanner;

        [SerializeField] private int tempOffsetX;
        [SerializeField] private int tempOffsetY;

        // Start is called before the first frame update
        void Start()
        {
            mainScanner = gameObject.GetComponent(typeof(LookScanner)) as LookScanner;

            StartCoroutine(ShootInterval());
            // when we want multithreading to make the scanner not frame dependent
            //thread = new Thread(new ThreadStart(ScanInterval));
            //thread.Start();
        }

        private IEnumerator ShootInterval()
        {
            while (true)
            {
                yield return null;
                ((IScans)mainScanner).Scan(aimPos);
            }
        }

        // Update is called once per frame
        void Update()
        {
            Ray eyePos = Camera.main.ScreenPointToRay(new Vector3((Screen.width / 2), (Screen.height / 2), 0));
            aimPos.x = eyePos.direction.x + tempOffsetX;
            aimPos.y = eyePos.direction.y + tempOffsetY;
        }
    }
}