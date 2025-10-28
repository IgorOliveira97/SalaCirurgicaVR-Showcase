using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// Classe responsável por gerenciar a progressão das cenas (enigmas) na simulação VR.
/// Controla a ativação de painéis, feedback háptico, dicas e tarefas.
/// Autor: [Seu Nome]
/// Projeto: [Nome do Projeto]
/// Ano: 2025
/// </summary>

public class Enigmas : MonoBehaviour
{
    #region Variáveis
    [Header("Referência do Script Dicas")]
    [SerializeField] private Dicas dicas;

    [Header("Referência do Script TimerCena")]
    [SerializeField] private TimerCena timers;

    [Header("GameObjects Adicionais")]
    [SerializeField] private GameObject paciente;
    [SerializeField] private GameObject instrum, cirurg, anest, circ;
    [SerializeField] private GameObject perg1;
    [SerializeField] private GameObject perg2;
    [SerializeField] private GameObject perg3;
    [SerializeField] private GameObject perg4;
    [SerializeField] private GameObject fim;
    [SerializeField] private GameObject bisturi;

    [Header("Referências de UI")]
    [SerializeField] private GameObject painelDicas;
    [SerializeField] private GameObject painelCena2;
    [SerializeField] private GameObject painelCena3;
    [SerializeField] private GameObject painelCena4;
    [SerializeField] private GameObject painelCena5;
    [SerializeField] private GameObject painelRevisao;
    [SerializeField] private GameObject painelFinal;
    [SerializeField] private GameObject painelTarefConcl;
    [SerializeField] private GameObject painelTarefConcl1;
    [SerializeField] private GameObject painelTempo;
    [SerializeField] private GameObject painelSemMover;
    [SerializeField] private GameObject textoTempo;
    public GameObject painelCena4Cirurgiao;
    public GameObject painelCena4Anestesista;
    public GameObject painelCena4Instrumentador;
    public GameObject painelCena4DiagAnest;
    public GameObject painelCena4DiagCirur;
    public GameObject painelCena5DiagCirc;

    [Header("Referência do MoveProvider")]
    [SerializeField] private ActionBasedContinuousMoveProvider moveProvider;

    [Header("Referência do XRController")]
    [SerializeField] private XRBaseController controllerLeft;

    private int valorEnigma = 0;
    private int contadorEnigma = 0;
    private int contadorCargo = 0;
    private string nomeAction = "";
    private bool setDef = false;
    private TextMeshPro textoDoPainel;
    #endregion

    //Desabilita o movimento do jogador e ativa painel com mensagem de movimento interrompido
    public void StopMovement()
    {
        moveProvider.enabled = false;
        painelSemMover.gameObject.SetActive(true);
    }

    //Reativa o movimento do jogador e desativa o painel com mensagem de movimento interrompido
    public void ResumeMovement()
    {
        moveProvider.enabled = true;
        painelSemMover.gameObject.SetActive(false);
    }

    //Recebe o nome da ação que está sendo executada, desativa o moveProvider e ativa o painelSemMover
    public void SetNomeAction(string nome)
    {
        nomeAction = nome;
        StopMovement();
    }

    //Reativa o moveProvider, desativa o painelSemMover e retorna o nome a ação
    public string GetNomeAction()
    {
        ResumeMovement();
        return nomeAction;
    }

    //Vibra o controle VR esquerdo
    public void VibrarLeft()
    {
        if (controllerLeft != null)
        {
            controllerLeft.SendHapticImpulse(0.5f, 0.7f);
        }
    }

    //Retorna o contador atual das tarefas realizadas
    public int ObterContador()
    {
        return contadorEnigma;
    }

    //Retorna o valor do enigma atual
    public int ObterEnigma()
    {
        return valorEnigma;
    }

    //Incrementa o contador de tarefas realizadas
    public void IncrementarContador()
    {
        contadorEnigma++;
    }

    //Incrementa o contador de cargos
    public void IncrementaContadorCargo()
    {
        contadorCargo++;
    }

    //Retorna o contador de cargos
    public int ObterContadorCargo()
    {
        return contadorCargo;
    }

    //Define a variável auxiliar setDef para verdadeiro
    public void SetDefTrue()
    {
        setDef = true;
    }

    //Retorna o valor atual da variável auxiliar setDef
    public bool GetDef()
    {
        return setDef;
    }

    //Recebe uma descrição de tarefa realizada e salva no texto do painel de tarefas concluidas.
    //Em seguida chama um método para ativar os painéis
    public void TempConcluido(string desc)
    {
        textoDoPainel.text= desc;
        StartCoroutine(MostrarPainelTemporario());
    }

    //Ativa os painéis de tarefa concluida por 5s e depois os desativa
    private IEnumerator MostrarPainelTemporario()
    {
        painelTarefConcl.gameObject.SetActive(true);
        painelTarefConcl1.gameObject.SetActive(true);
        yield return new WaitForSeconds(5f);
        painelTarefConcl.gameObject.SetActive(false);
        painelTarefConcl1.gameObject.SetActive(false);
    }

    //Configura o texto do botão dicas
    public void ConfiguraTexto()
    {
        dicas.texto1.fontSize = 13;
        dicas.textoBotaoDicas1.text = "Pedir Dica";
    }

    //Desativa elementos da cena 1 e ativa os da cena 2
    private void AtivaCena2()
    {
        perg1.SetActive(false);
        perg2.SetActive(true);
        timers.DeslTimer();
        painelCena2.gameObject.SetActive(true);
        VibrarLeft();
        StopMovement();
        paciente.transform.Find("Cover").gameObject.SetActive(true);
        paciente.transform.Find("Patient").gameObject.SetActive(true);
        instrum.SetActive(true);
        cirurg.SetActive(true);
        anest.SetActive(true);
        circ.SetActive(true);
        ConfiguraTexto();
        dicas.dicasRestantes = 7;
        dicas.rangeMax = 7;
    }

    //Desativa elementos da cena 2 e ativa os da cena 3
    private void AtivaCena3()
    {
        perg2.SetActive(false);
        timers.DeslTimer();
        painelCena3.gameObject.SetActive(true);
        VibrarLeft();
        StopMovement();
        painelDicas.gameObject.SetActive(false);
    }

    //Desativa elementos da cena 3 e ativa os da cena 4
    private void AtivaCena4()
    {
        perg3.SetActive(true);
        timers.DeslTimer();
        painelCena4.gameObject.SetActive(true);
        VibrarLeft();
        StopMovement();
        painelDicas.gameObject.SetActive(true);
        ConfiguraTexto();
        dicas.dicasRestantes = 3;
        dicas.rangeMax = 3;
        painelCena4DiagCirur.gameObject.SetActive(true);
    }

    //Desativa elementos da cena 4 e ativa os da cena 5
    private void AtivaCena5()
    {
        perg3.SetActive(false);
        perg4.SetActive(true);
        timers.DeslTimer();
        painelCena5.gameObject.SetActive(true);
        VibrarLeft();
        StopMovement();
        bisturi.transform.Find("smoke_thin").gameObject.SetActive(true);
        ConfiguraTexto();
        dicas.dicasRestantes = 3;
        dicas.rangeMax = 3;
        painelCena5DiagCirc.gameObject.SetActive(true);
    }

    //Desativa elementos da cena 5 e ativa os da cena final
    private void AtivaFinal()
    {
        perg4.SetActive(false);
        fim.SetActive(true);
        timers.DeslTimer();
        painelDicas.gameObject.SetActive(false);
        painelFinal.gameObject.SetActive(true);
        VibrarLeft();
    }

    //Gerencia as Cenas (Enigmas) da simulação
    public void ApagaEnigma()
    {
        switch (valorEnigma)
        {
            case 0:
                AtivaCena2();
                valorEnigma++;
                break;
            case 1:
                AtivaCena3();
                valorEnigma++;
                break;
            case 2:
                AtivaCena4();
                valorEnigma++;
                break;
            case 3:
                AtivaCena5();
                valorEnigma++;
                break;
            case 4:
                painelRevisao.gameObject.SetActive(true);
                VibrarLeft();
                valorEnigma++;
                break;
            case 5:
                AtivaFinal();
                valorEnigma++;
                break;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //Salva o texto do painel de tarefas e esconde o tempo e o painel na cena
        textoDoPainel = painelTarefConcl1.GetComponentInChildren<TextMeshPro>();
        textoTempo.gameObject.GetComponent<MeshRenderer>().enabled = false;
        CanvasGroup cg = painelTempo.GetComponent<CanvasGroup>();
        cg.alpha = 0f;
        cg.interactable = false;
        cg.blocksRaycasts = false;

    }

}
