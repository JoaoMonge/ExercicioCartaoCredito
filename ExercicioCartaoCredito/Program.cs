using System.Text.RegularExpressions;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

DateOnly validade = new DateOnly(DateOnly.FromDateTime(DateTime.Today).Year + 6, DateOnly.FromDateTime(DateTime.Today).Month, DateOnly.FromDateTime(DateTime.Today).Day);
CartaoCredito cartao1 = new CartaoCredito("Joao Pedro", 120000123012,validade,4500.0);

cartao1.gastar("Roupa", 2000.0);
cartao1.gastar("Seguro", 200.0);
cartao1.gastar("Supermercado", 1000.0);
cartao1.gastar("Luz", 300.0);
cartao1.gastar("Gás", 1000.0);
cartao1.gastar("Carro Novo", 5000.0);
cartao1.pagarCredito(50000.0);
cartao1.gastar("Carro Novo", 3500.0);

CartaoCredito cartao2 = new CartaoCredito("Maria Pedro", 120000123012, validade, 500.0);

cartao2.gastar("Supermercado", 150.0);
cartao2.gastar("Seguros", 150.0);
cartao2.gastar("Contas", 250.0);

Carteira cart = new Carteira("Familia Pedro", "Rua x", "913213213");
cart.addCard(cartao1);
cart.addCard(cartao2);



app.MapGet("/", () => cart.toString());

app.Run();

class CartaoCredito {

    private String titular;
    private long numeroConta;
    private DateOnly validade;
    private double maximoAutorizado;
    private double montanteGasto = 0.0;

    private List<String> movimentos = new List<String>();

  

    public CartaoCredito(String titular,
        long numeroConta, DateOnly validade,
       double maximoAutorizado) {

        this.titular = titular;

        if (Regex.Matches("" + numeroConta, @"[0-9]{12}").Count > 0)
        {
            this.numeroConta = numeroConta;
        }
        else
        {
            throw new Exception("Numero de conta inválido.");
        }
        this.validade = validade;
        this.maximoAutorizado = maximoAutorizado;

    }

    public double saldo() {

        //O montante máximo disponivel no cartão menos o montante que já gastamos.
        return maximoAutorizado - montanteGasto;

    }

    public void pagarCredito(double valor)
    {
        String mov = "";
        if (montanteGasto >= valor)
        {
            montanteGasto -= valor;
            mov = "+" + (valor) + "€ - Liquidação de divida";
        }
        else {
            mov = "+" + (montanteGasto) + "€ - Liquidação de divida";
            montanteGasto = 0;
        }

        movimentos.Add(mov);
    }

    public void gastar(String descricao, double valor) {

        if (valor <= saldo())
        {
            this.montanteGasto += valor;
            String mov = "-" + valor + "€ - " + descricao;
            movimentos.Add(mov);

        }
        else {
            Console.WriteLine("Saldo indisponivel.");
        }

    }

    public String obterTalao()
    {

        return movimentos.Last<String>();

    }

    public String getMovimentos()
    {
        String movs = "";
        foreach(String movimento in movimentos)
        {
            movs += movimento + "\n";
        }
        return movs;
    }

    public String toString()
    {
        String dados = numeroConta + "\n" + titular + "       " + validade.Month + "/" + validade.Year + "\n";
        Console.WriteLine(dados);
        return dados;
    }

    public override bool Equals(object? obj)
    {
        return obj is CartaoCredito credito &&
               numeroConta == credito.numeroConta;
    }

    public double MontanteGasto { get => montanteGasto; }

}

/*
 
 2. Considere agora que se pretende reproduzir a estrutura de uma carteira com espaço para cartões
A carteira deve ter dados sobre o dono (nome, morada, número de telefone), assim como os cartões que​ ​ele​ ​p
ossui.​
Crie​ ​a​ ​classe​ ​​Carteira​,​ ​onde​ ​deve​ ​ser​ ​possível​ ​efetuar​ ​o​ ​seguinte:

●  Guardar​ ​mais​ ​um​ ​cartão;

●  Listar​ ​todos​ ​os​ ​cartões​ ​existentes;

●  Retirar um cartão da carteira, com base no seu número

   Atenção que tal só deverá ser possível​ ​caso​ ​o​ ​montante​ ​em​ ​dívida​ ​no​ ​cartão​ ​seja​ ​igual​ ​a​ ​zero;

●  Determinar​ ​quantos​ ​cartões​ ​estão​ ​na​ ​carteira;

●  Determinar​ ​o​ ​montante​ ​em​ ​dívida​ ​na​ ​carteira;

●  Devolver,​ ​sob​ ​a​ ​forma​ ​de​ ​String,​ ​todos​ ​os​ ​movimentos​ ​de​ ​cartões​ ​atualmente​ ​na​ ​carteira;

●  Representar uma carteira sob a forma de String (com informações sobre a carteira e também​ ​sobre​ ​os​ ​cartões​ ​e​ ​respetivo​ ​saldo​ ​devedor).


 */
class Carteira
{
    String nome;
    String morada;
    String contacto;


    List<CartaoCredito> cartoes = new List<CartaoCredito>();


    public Carteira(String nome, String morada, String contacto) {

        this.nome = nome;
        this.morada = morada;
        if (Regex.Matches(contacto,@"9[1236][0-9]{7}|2[1-9][0-9]{7}|707[0-9]{6}|808[0-9]{6}|800[0-9]{6}").Count > 0)
        {
            this.contacto = contacto;
        }
        else
        {
            //Lança um erro
            throw new Exception("Erro: Contacto Inválido");
        }

    }

    public void addCard(CartaoCredito card)
    {
        cartoes.Add(card);
    }

    public void removeCard(long numeroCartao) {
        
        CartaoCredito toRemove = null;
        foreach (CartaoCredito c in cartoes)
        {
            CartaoCredito tmp = new CartaoCredito("", numeroCartao, new DateOnly(), 0.0);
            if (c.Equals(tmp) ) {
                 toRemove = c;
                 break;
            }
        }
        if (toRemove != null)
        {
            if (toRemove.MontanteGasto <= 0) {
                cartoes.Remove(toRemove);
            }
            else
            {
                Console.WriteLine("Liquide o montante em divida primeiro.");
            }
        }
        else {
            Console.WriteLine("Não encontrado!!");
        }
      
    }

    public int quantidadeCartoes()
    {
        return this.cartoes.Count;
    }

    public double montanteEmDivida()
    {
        double divida = 0.0;

        foreach(CartaoCredito c in this.cartoes)
        {
            divida += c.MontanteGasto;
        }

        return divida;
    }

    public String movimentos()
    {
        String movs = "";

        foreach(CartaoCredito c in this.cartoes)
        {

            movs += c.getMovimentos();
        }

        return movs;
    }

    public String toString()
    {
        String dados = "DADOS:\nNome: " + nome + "\nMorada: " + morada + "\nContacto: " + contacto + "\n";
        String cartoes = "\nCARTÕES:\n";

        foreach(CartaoCredito c in this.cartoes)
        {
            cartoes += c.toString();
        }

        String divida = "\nDIVIDA:\n" + montanteEmDivida() + "€";

        return dados + cartoes + divida;

    }
}