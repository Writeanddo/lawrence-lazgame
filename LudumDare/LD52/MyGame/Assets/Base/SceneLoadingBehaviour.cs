using DG.Tweening;
using DG.Tweening.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Libs.Base.GameLogic
{
    public class SceneLoadingBehaviour : MonoBehaviour
    {
        public string SceneName = "";
        public int SceneOffset = 1;

        private void Update()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.F5))
            {
                RestartScene();
            }
            //else if (UnityEngine.Input.GetKeyDown(KeyCode.Escape))
            //{
            //    var music = FindObjectOfType<Music>();
            //    if (music != null)
            //    {
            //        Destroy(music.gameObject);
            //    }
            //    ScreenFade.Instance.FadeOut();
            //    DOTween.Sequence()
            //    .AppendInterval(1)
            //    .AppendCallback(() => Application.Quit());
            //}
        }

        [ContextMenu("LoadScene")]
        public void LoadScene()
        {
            if (!string.IsNullOrWhiteSpace(SceneName))
            {
                LoadScene(SceneName);
            }
            else
            {
                if (load != null)
                    return;

                ScreenFade.Instance.FadeOut();
                load = DOTween.Sequence()
                    .AppendInterval(1)
                    .AppendCallback(() =>
                    {
                        DOTween.KillAll(false);
                        Destroy(FindObjectOfType<DOTweenComponent>().gameObject);
                        SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex + SceneOffset) % SceneManager.sceneCountInBuildSettings);
                    });
            }
        }

        [ContextMenu("LoadSceneAsync")]
        public void LoadSceneAsync()
        {
            DOTween.KillAll(false);
            Destroy(FindObjectOfType<DOTweenComponent>().gameObject);
            SceneManager.LoadSceneAsync(SceneName);
        }

        Sequence load;

        public void LoadScene(string name)
        {
            if (load != null)
                return;

            ScreenFade.Instance.FadeOut();
            load = DOTween.Sequence()
                .AppendInterval(1)
                .AppendCallback(() =>
                {
                    DOTween.KillAll(false);
                    Destroy(FindObjectOfType<DOTweenComponent>().gameObject);
                    SceneManager.LoadScene(name);
                });
        }

        public void RestartScene()
        {
            if (load != null)
                return;

            load = DOTween.Sequence()
                .AppendInterval(UnityEngine.Input.GetKeyDown(KeyCode.R) ? 0f : 0.8f)
                .AppendCallback(() => ScreenFade.Instance.FadeOut(.3f))
                .AppendInterval(0.2f)
                .AppendCallback(() =>
                {
                    DOTween.KillAll(false);
                    Destroy(FindObjectOfType<DOTweenComponent>().gameObject);
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                });
        }
    }
}
