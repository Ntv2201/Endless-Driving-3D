using System.Collections;
using UnityEngine;

public class EndlessLevelHandler : MonoBehaviour
{
    [SerializeField] GameObject[] sectionPrefabs;
    [SerializeField] GameObject[] sectionPool = new GameObject[20];
    [SerializeField] GameObject[] sectionsToRender = new GameObject[10];
    private Transform playerCarTransform;
    WaitForSeconds waitFor100ms = new WaitForSeconds(0.1f); 
    const float sectionLength = 26;

    private void Start()
    {
        playerCarTransform = GameObject.FindGameObjectWithTag("Player").transform;

        GenerateMap();

    }

    private void Update()
    {
        
    }

    private void GenerateMap()
    {
        int prefabIndex = 0;

        for(int i = 0; i < sectionPool.Length; i++)
        {
            sectionPool[i] = Instantiate(sectionPrefabs[prefabIndex]);
            sectionPool[i].SetActive(false);

            prefabIndex++;

            if(prefabIndex > sectionPrefabs.Length - 1)
            {
                prefabIndex = 0;
            }
        }

        for(int i = 0; i < sectionsToRender.Length; i++)
        {
            // get a random section
            GameObject randomSection = GetRandomSectionFromPool();
            
            // move it into position and active it
            randomSection.transform.position = new Vector3(sectionPool[i].transform.position.x, 0, i * sectionLength);
            randomSection.SetActive(true);

            // set the section in the array
            sectionsToRender[i] = randomSection;
        }
        StartCoroutine(UpdateCoroutine());
    }

    private IEnumerator UpdateCoroutine()
    {
        while (true)
        {
            UpdateSectionPosition();
            yield return waitFor100ms;
        }
    }

    private void UpdateSectionPosition()
    {
        for(int i = 0; i < sectionsToRender.Length; i++)
        {   
            // check if the section is far behind
            if(sectionsToRender[i].transform.position.z - playerCarTransform.transform.position.z < -sectionLength)
            {
                // store the position of the section and disable it
                Vector3 lastSectionPosition = sectionsToRender[i].transform.position;
                sectionsToRender[i].SetActive(false);

                // get a new section, enable it and move it forward
                sectionsToRender[i] = GetRandomSectionFromPool();

                // move the new section into place and active it
                sectionsToRender[i].transform.position = new Vector3(
                    lastSectionPosition.x, 0, 
                    lastSectionPosition.z + sectionLength * sectionsToRender.Length
                    );
                sectionsToRender[i].SetActive(true);
            }
        }
    }

    private GameObject GetRandomSectionFromPool()
    {
        // pick a random index that is available
        int randomIndex = Random.Range(0, sectionPool.Length);

        bool isNewSectionFound = false;

        while (!isNewSectionFound)
        {
            // check if the section is not set active, in that case we've found a section
            if (!sectionPool[randomIndex].activeInHierarchy)
            {
                isNewSectionFound = true;
            }
            else
            {
                // if it was active, we'll find another, so we increase the index
                randomIndex++;

                // ensure that we loop around if we reach the end of the array
                if(randomIndex > sectionPool.Length - 1)
                {
                    randomIndex = 0;
                }
            }
        }

        return sectionPool[randomIndex];
    }

}
