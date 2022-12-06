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


app.MapGet("/", () =>"Movimentos:\n"+ cartao1.obterTalao());

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
        this.numeroConta = numeroConta;
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


}