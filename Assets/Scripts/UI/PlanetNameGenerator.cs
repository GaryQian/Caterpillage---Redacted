using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetNameGenerator : MonoBehaviour {

    public static string lastNameGenerated;

    public static ArrayList suffixes;
    public static ArrayList prefixes;
    public static ArrayList middle;
    public static string[] capitalNamesStatic;
    public static string[] colonyNamesStatic;

    public string[] capitalNames;
    public string[] colonyNames;



    private void Start() {
        Init();
        capitalNamesStatic = capitalNames;
        colonyNamesStatic = colonyNames;
    }

    private static void Init() {
        suffixes = new ArrayList();
        suffixes.Add("us");
        suffixes.Add("lon");
        suffixes.Add("nia");
        suffixes.Add("thea");
        suffixes.Add("age");
        suffixes.Add("ius");
        suffixes.Add("phalon");
        suffixes.Add("frey");
        suffixes.Add("ster");
        suffixes.Add("sium");
        suffixes.Add("ium");
        suffixes.Add("urn");
        suffixes.Add("ict");
        suffixes.Add("mia");
        suffixes.Add("tus");
        suffixes.Add("ictus");
        suffixes.Add("num");
        suffixes.Add("ama");
        suffixes.Add("ara");
        suffixes.Add("cer");
        suffixes.Add("tavim");
        suffixes.Add("ture");
        suffixes.Add("sie");
        suffixes.Add("ine");
        suffixes.Add("too");
        suffixes.Add("ctum");
        suffixes.Add("turn");
        suffixes.Add("tim");
        suffixes.Add("old");
        suffixes.Add("qua");
        suffixes.Add("quar");
        suffixes.Add("zar");
        suffixes.Add("zan");
        suffixes.Add("phor");
        suffixes.Add("tune");
        suffixes.Add("cury");
        suffixes.Add("ars");
        suffixes.Add("ter");
        suffixes.Add("scant");
        suffixes.Add("and");
        suffixes.Add("ant");
        suffixes.Add("wyd");
        suffixes.Add("ca");
        suffixes.Add("port");
        suffixes.Add("each");
        suffixes.Add("en");
        suffixes.Add("ader");
        suffixes.Add("var");
        suffixes.Add("ine");
        suffixes.Add("ock");
        suffixes.Add("topia");
        suffixes.Add("ard");
        suffixes.Add("eme");
        suffixes.Add("eden");
        suffixes.Add("phar");
        suffixes.Add("ante");
        suffixes.Add("ander");
        suffixes.Add("muth");
        suffixes.Add("bium");
        suffixes.Add("ard");
        suffixes.Add("lin");
        suffixes.Add("on");
        suffixes.Add("ame");
        suffixes.Add("tier");
        suffixes.Add("cia");
        suffixes.Add("macia");
        suffixes.Add("acy");
        suffixes.Add("ican");
        suffixes.Add("ig");
        suffixes.Add("ins");
        suffixes.Add("kin");
        //suffixes.Add("");
        //suffixes.Add("");
        //suffixes.Add("");
        //suffixes.Add("");


        prefixes = new ArrayList();
        prefixes.Add("Cor");
        prefixes.Add("Havi");
        prefixes.Add("Zan");
        prefixes.Add("Sat");
        prefixes.Add("Hero");
        prefixes.Add("Merc");
        prefixes.Add("Mans");
        prefixes.Add("Mons");
        prefixes.Add("Nec");
        prefixes.Add("Quan");
        prefixes.Add("Gar");
        prefixes.Add("Ram");
        prefixes.Add("Riv");
        prefixes.Add("Url");
        prefixes.Add("Sand");
        prefixes.Add("Gali");
        prefixes.Add("Prosp");
        prefixes.Add("Ill");
        prefixes.Add("Eles");
        prefixes.Add("Rap");
        prefixes.Add("Illi");
        prefixes.Add("Ove");
        prefixes.Add("Nor");
        prefixes.Add("Baum");
        prefixes.Add("Nath");
        prefixes.Add("Weir");
        prefixes.Add("Vol");
        prefixes.Add("Vard");
        prefixes.Add("Cap");
        prefixes.Add("Card");
        prefixes.Add("Tran");
        prefixes.Add("Coru");
        prefixes.Add("Aber");
        prefixes.Add("Atri");
        prefixes.Add("Orphe");
        prefixes.Add("Kra");
        prefixes.Add("Live");
        prefixes.Add("Lus");
        prefixes.Add("Lum");
        prefixes.Add("Veri");
        prefixes.Add("Aqua");
        prefixes.Add("Phob");
        prefixes.Add("Lan");
        prefixes.Add("Landis");
        prefixes.Add("Indo");
        prefixes.Add("Ameri");
        prefixes.Add("Ford");
        prefixes.Add("Fren");
        prefixes.Add("Free");
        prefixes.Add("R");
        prefixes.Add("Ward");
        prefixes.Add("Aber");
        prefixes.Add("Xan");
        prefixes.Add("Xath");
        prefixes.Add("Zyri");
        prefixes.Add("Grand");
        prefixes.Add("Gre");
        prefixes.Add("Dro");
        prefixes.Add("Dath");
        prefixes.Add("Darth");
        prefixes.Add("Far");
        prefixes.Add("Jin");
        prefixes.Add("Jour");
        prefixes.Add("Jar");
        prefixes.Add("Mur");
        prefixes.Add("Muir");
        prefixes.Add("Star");
        prefixes.Add("Coru");
        prefixes.Add("Nove");
        prefixes.Add("Gari");
        prefixes.Add("Jeri");
        prefixes.Add("Melo");
        prefixes.Add("Garde");
        prefixes.Add("Ahr");
        prefixes.Add("Jin");
        prefixes.Add("Kron");
        prefixes.Add("Die");
        prefixes.Add("Dim");
        prefixes.Add("Narg");
        prefixes.Add("Ravi");
        prefixes.Add("Fron");
        prefixes.Add("Wh");
        prefixes.Add("Demo");
        prefixes.Add("Rep");
        prefixes.Add("Com");
        prefixes.Add("Uni");
        prefixes.Add("Hop");


        middle = new ArrayList();
        middle.Add("ara");
        middle.Add("ti");
        middle.Add("rav");
        middle.Add("lor");
        middle.Add("cri");
        middle.Add("cree");
        middle.Add("tor");
        middle.Add("mon");
        middle.Add("ran");
        middle.Add("eve");
        middle.Add("ia");
        middle.Add("nan");
        middle.Add("hur");
        middle.Add("sta");
        middle.Add("or");
        middle.Add("iu");
        middle.Add("no");
        middle.Add("do");
        middle.Add("bl");
        middle.Add("us");
        middle.Add("vi");
        middle.Add("is");
        middle.Add("fr");
        middle.Add("qu");
        middle.Add("mi");
        middle.Add("ch");
        middle.Add("th");
        middle.Add("var");
        middle.Add("vin");
        middle.Add("sto");
        middle.Add("ri");
        middle.Add("cra");
        middle.Add("ar");
        middle.Add("tter");
        middle.Add("on");
        middle.Add("bor");
        middle.Add("too");
        middle.Add("var");
        middle.Add("m");
        middle.Add("i");
        middle.Add("pub");
        middle.Add("cra");
        middle.Add("bur");
        middle.Add("fav");
        middle.Add("um");
        middle.Add("nos");
        middle.Add("das");
        middle.Add("li");
        middle.Add("bra");
        middle.Add("c");
        middle.Add("mu");
        middle.Add("hi");
        middle.Add("en");
        middle.Add("ar");
        middle.Add("at");
        middle.Add("th");
        middle.Add("im");
        middle.Add("sh");
        middle.Add("bl");

        //prefixes.Add("");
        //prefixes.Add("");

    }

    public static string NewName(int i = 0) {
        if (middle == null) { Init();  }
        if (i >= 80 && i < 90) {
            lastNameGenerated = colonyNamesStatic[i - 80];
        }
        else if (i >= 90 && i < 101) {
            lastNameGenerated = capitalNamesStatic[i - 90];
        }
        else {
            lastNameGenerated = (string)prefixes[Random.Range(0, prefixes.Count)] +
                (Random.Range(0, 1f) < 0.3f ? (string)middle[Random.Range(0, middle.Count)] : "") +
                (string)suffixes[Random.Range(0, suffixes.Count)];
        }
        return lastNameGenerated;
    }



}
