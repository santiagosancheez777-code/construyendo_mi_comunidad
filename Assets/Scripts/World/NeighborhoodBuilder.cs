using UnityEngine;

namespace ConstruyendoMiComunidad
{
    /// <summary>
    /// Construye el barrio low-poly con primitivas de Unity.
    /// </summary>
    public static class NeighborhoodBuilder
    {
        static CommunityVisualSettings VisualSettings;
        static Color Cesped = new Color(0.45f, 0.72f, 0.32f);
        static readonly Color Tierra = new Color(0.55f, 0.42f, 0.28f);
        static Color Calle = new Color(0.35f, 0.36f, 0.38f);
        static Color Anden = new Color(0.72f, 0.71f, 0.68f);
        static Color ParedA = new Color(0.95f, 0.78f, 0.42f);
        static Color ParedB = new Color(0.93f, 0.55f, 0.45f);
        static Color ParedC = new Color(0.55f, 0.72f, 0.88f);
        static Color Techo = new Color(0.75f, 0.28f, 0.22f);
        static readonly Color Tronco = new Color(0.45f, 0.28f, 0.16f);
        static Color Copa = new Color(0.22f, 0.55f, 0.28f);
        static Color Centro = new Color(0.95f, 0.92f, 0.82f);
        static readonly Color Reciclaje = new Color(0.2f, 0.45f, 0.85f);
        static readonly Color Organico = new Color(0.25f, 0.62f, 0.28f);

        public static Transform Build()
        {
            VisualSettings = Resources.Load<CommunityVisualSettings>("CommunityVisualSettings");
            if (VisualSettings != null)
            {
                Cesped = VisualSettings.grass;
                Calle = VisualSettings.road;
                Anden = VisualSettings.sidewalk;
                ParedA = VisualSettings.houseWallA;
                ParedB = VisualSettings.houseWallB;
                ParedC = VisualSettings.houseWallC;
                Techo = VisualSettings.roof;
                Copa = VisualSettings.treeCanopy;
                Centro = VisualSettings.communityCenter;
            }
            var root = new GameObject("NeighborhoodRoot").transform;

            Prim(root, PrimitiveType.Cube, new Vector3(0f, -0.05f, 0f), new Vector3(90f, 0.1f, 90f), Cesped, "Suelo");

            // Calles en cruz y anillo
            Prim(root, PrimitiveType.Cube, new Vector3(0f, 0.02f, 0f), new Vector3(8f, 0.06f, 80f), Calle, "CalleNorteSur");
            Prim(root, PrimitiveType.Cube, new Vector3(0f, 0.02f, 0f), new Vector3(80f, 0.06f, 8f), Calle, "CalleEsteOeste");
            Prim(root, PrimitiveType.Cube, new Vector3(-6.5f, 0.03f, 0f), new Vector3(2.2f, 0.05f, 80f), Anden, "AndenOeste");
            Prim(root, PrimitiveType.Cube, new Vector3(6.5f, 0.03f, 0f), new Vector3(2.2f, 0.05f, 80f), Anden, "AndenEste");

            Casa(root, new Vector3(-18f, 0f, -16f), ParedA);
            Casa(root, new Vector3(-18f, 0f, -8f), ParedB);
            Casa(root, new Vector3(-18f, 0f, 8f), ParedC);
            Casa(root, new Vector3(-18f, 0f, 16f), ParedA);
            Casa(root, new Vector3(18f, 0f, -16f), ParedC);
            Casa(root, new Vector3(18f, 0f, -8f), ParedA);
            Casa(root, new Vector3(18f, 0f, 8f), ParedB);
            Casa(root, new Vector3(18f, 0f, 16f), ParedC);
            Casa(root, new Vector3(-10f, 0f, 22f), ParedB);
            Casa(root, new Vector3(10f, 0f, 22f), ParedA);
            Casa(root, new Vector3(-10f, 0f, -24f), ParedC);
            Casa(root, new Vector3(10f, 0f, -24f), ParedB);

            Arbol(root, new Vector3(-12f, 0f, -12f));
            Arbol(root, new Vector3(12f, 0f, -12f));
            Arbol(root, new Vector3(-12f, 0f, 12f));
            Arbol(root, new Vector3(12f, 0f, 12f));
            Arbol(root, new Vector3(-22f, 0f, 0f));
            Arbol(root, new Vector3(22f, 0f, 4f));
            Arbol(root, new Vector3(0f, 0f, 28f));
            Arbol(root, new Vector3(-4f, 0f, -28f));
            Farola(root, new Vector3(-6.6f, 0f, -18f));
            Farola(root, new Vector3(6.6f, 0f, -18f));
            Farola(root, new Vector3(-6.6f, 0f, 18f));
            Farola(root, new Vector3(6.6f, 0f, 18f));

            var parqueSucio = new GameObject("ParqueSucio");
            parqueSucio.transform.SetParent(root);
            Prim(parqueSucio.transform, PrimitiveType.Cube, new Vector3(0f, 0.04f, 0f), new Vector3(14f, 0.08f, 14f), Tierra, "PisoParqueSucio");

            var parqueLimpio = new GameObject("ParqueLimpio");
            parqueLimpio.transform.SetParent(root);
            parqueLimpio.SetActive(false);
            Prim(parqueLimpio.transform, PrimitiveType.Cube, new Vector3(0f, 0.05f, 0f), new Vector3(14f, 0.08f, 14f), new Color(0.38f, 0.78f, 0.36f), "PisoParqueLimpio");
            Banco(parqueLimpio.transform, new Vector3(-3.5f, 0f, 3.5f));
            Banco(parqueLimpio.transform, new Vector3(3.5f, 0f, 3.5f));
            Prim(parqueLimpio.transform, PrimitiveType.Cylinder, new Vector3(0f, 0.35f, 0f), new Vector3(1.6f, 0.35f, 1.6f), new Color(0.55f, 0.75f, 0.95f), "Fuente");

            // Centro comunitario (norte)
            Prim(root, PrimitiveType.Cube, new Vector3(0f, 1.6f, 20f), new Vector3(10f, 3.2f, 6f), Centro, "CentroComunitario");
            Prim(root, PrimitiveType.Cube, new Vector3(0f, 3.5f, 20f), new Vector3(11f, 0.4f, 7f), Techo, "TechoCentro");
            var puerta = Prim(root, PrimitiveType.Cube, new Vector3(0f, 1f, 16.6f), new Vector3(3.2f, 2f, 1.2f), new Color(0.4f, 0.28f, 0.16f), "EntradaAsamblea");
            puerta.AddComponent<AssemblyTrigger>();
            puerta.GetComponent<BoxCollider>().isTrigger = true;

            // Tablero PUI y cuestionario junto al centro
            var tablero = Prim(root, PrimitiveType.Cube, new Vector3(-6f, 1.4f, 17.5f), new Vector3(0.2f, 2.2f, 2.4f), new Color(0.15f, 0.35f, 0.55f), "TableroMedellin");
            tablero.AddComponent<MedellinBoard>();
            tablero.GetComponent<BoxCollider>().isTrigger = true;

            var quiz = Prim(root, PrimitiveType.Cube, new Vector3(6f, 1.4f, 17.5f), new Vector3(0.2f, 2.2f, 2.4f), new Color(0.55f, 0.35f, 0.15f), "TableroCuestionario");
            quiz.AddComponent<QuizTrigger>();
            quiz.GetComponent<BoxCollider>().isTrigger = true;

            Caneca(root, new Vector3(-5.5f, 0.55f, -6.5f), TrashKind.Reciclable, Reciclaje, "CanecaReciclaje");
            Caneca(root, new Vector3(5.5f, 0.55f, -6.5f), TrashKind.Organico, Organico, "CanecaOrganica");

            Vector3[] basuras =
            {
                new Vector3(-2.5f, 0.25f, 1.5f),
                new Vector3(2.2f, 0.25f, -2.1f),
                new Vector3(-3.8f, 0.25f, -3.2f),
                new Vector3(1.4f, 0.25f, 3.6f),
                new Vector3(3.9f, 0.25f, 1.1f),
                new Vector3(-1.1f, 0.25f, -4.4f),
                new Vector3(0.6f, 0.25f, 4.8f),
                new Vector3(-4.4f, 0.25f, 2.8f)
            };

            for (int i = 0; i < basuras.Length; i++)
            {
                var tipo = i % 2 == 0 ? TrashKind.Reciclable : TrashKind.Organico;
                var go = new GameObject("Residuo_" + i);
                go.transform.SetParent(parqueSucio.transform);
                go.transform.position = basuras[i];
                go.transform.rotation = Quaternion.Euler(8f, i * 35f, 12f);
                var color = tipo == TrashKind.Reciclable ? new Color(0.12f, 0.58f, 0.72f) : new Color(0.82f, 0.62f, 0.24f);
                if (tipo == TrashKind.Reciclable)
                {
                    if (i % 4 == 0)
                    {
                        // Botella PET con cuello y tapa contrastante.
                        SinCollider(Prim(go.transform, PrimitiveType.Cylinder, basuras[i], new Vector3(0.22f, 0.3f, 0.22f), color, "Botella"));
                        SinCollider(Prim(go.transform, PrimitiveType.Cylinder, basuras[i] + new Vector3(0f, 0.31f, 0f), new Vector3(0.11f, 0.12f, 0.11f), color, "CuelloBotella"));
                        SinCollider(Prim(go.transform, PrimitiveType.Cylinder, basuras[i] + new Vector3(0f, 0.44f, 0f), new Vector3(0.13f, 0.05f, 0.13f), new Color(0.95f, 0.95f, 0.9f), "TapaBotella"));
                        SinCollider(Prim(go.transform, PrimitiveType.Cube, basuras[i] + new Vector3(0f, 0.02f, 0.225f), new Vector3(0.2f, 0.13f, 0.018f), new Color(0.82f, 0.92f, 0.88f), "EtiquetaBotella"));
                    }
                    else
                    {
                        // Lata aplastada con borde superior e inferior metálicos.
                        var lata = Prim(go.transform, PrimitiveType.Cylinder, basuras[i], new Vector3(0.26f, 0.2f, 0.26f), new Color(0.65f, 0.7f, 0.71f), "Lata");
                        lata.GetComponent<Renderer>().sharedMaterial = WorldPalette.Metal(new Color(0.65f, 0.7f, 0.71f));
                        SinCollider(lata);
                        var bordeLata = Prim(go.transform, PrimitiveType.Cylinder, basuras[i] + new Vector3(0f, 0.2f, 0f), new Vector3(0.27f, 0.025f, 0.27f), new Color(0.82f, 0.85f, 0.83f), "BordeLata");
                        bordeLata.GetComponent<Renderer>().sharedMaterial = WorldPalette.Metal(new Color(0.82f, 0.85f, 0.83f));
                        SinCollider(bordeLata);
                        SinCollider(Prim(go.transform, PrimitiveType.Cube, basuras[i] + new Vector3(0f, 0.04f, 0.265f), new Vector3(0.22f, 0.16f, 0.02f), color, "EtiquetaLata"));
                    }
                }
                else
                {
                    if (i % 4 == 1)
                    {
                        // Cáscara de banano y tallo.
                        SinCollider(Prim(go.transform, PrimitiveType.Sphere, basuras[i], new Vector3(0.55f, 0.2f, 0.24f), color, "Banano"));
                        SinCollider(Prim(go.transform, PrimitiveType.Cylinder, basuras[i] + new Vector3(0.22f, 0.12f, 0f), new Vector3(0.07f, 0.08f, 0.07f), new Color(0.3f, 0.48f, 0.2f), "Tallo"));
                    }
                    else
                    {
                        // Manzana con depresión superior y hoja.
                        SinCollider(Prim(go.transform, PrimitiveType.Sphere, basuras[i], new Vector3(0.38f, 0.34f, 0.36f), new Color(0.72f, 0.28f, 0.2f), "Manzana"));
                        SinCollider(Prim(go.transform, PrimitiveType.Cylinder, basuras[i] + new Vector3(0f, 0.28f, 0f), new Vector3(0.045f, 0.1f, 0.045f), new Color(0.3f, 0.25f, 0.16f), "TalloManzana"));
                        SinCollider(Prim(go.transform, PrimitiveType.Sphere, basuras[i] + new Vector3(0.12f, 0.3f, 0f), new Vector3(0.18f, 0.06f, 0.09f), new Color(0.35f, 0.55f, 0.22f), "HojaManzana"));
                    }
                }
                QuitarCollidersDeDecoracion(go.transform);
                var hitbox = go.AddComponent<BoxCollider>();
                hitbox.size = new Vector3(0.9f, 0.8f, 0.8f);
                hitbox.isTrigger = true;
                var item = go.AddComponent<TrashItem>();
                item.Tipo = tipo;
            }

            CrearNpc(root, 0, "carmen", "Doña Carmen", new Vector3(-7f, 0f, 2f), new Color(0.85f, 0.45f, 0.55f),
                "El parque está abandonado y sucio. Los niños ya no vienen a jugar y da tristeza verlo así.",
                "Hola. Vivo aquí desde hace treinta años. El parque se llenó de residuos y nadie se siente dueño de él. Si nos organizamos, podríamos recuperarlo entre todos.");

            CrearNpc(root, 1, "luis", "Don Luis", new Vector3(8f, 0f, -3f), new Color(0.35f, 0.5f, 0.75f),
                "Las calles tienen residuos y eso afecta la salud de todas las personas.",
                "Las calles se llenaron de bolsas y escombros. No es solo un problema de aseo: es de salud y de dignidad. Una jornada comunitaria de limpieza cambiaría el barrio.");

            CrearNpc(root, 2, "valentina", "Valentina", new Vector3(4f, 0f, 8f), new Color(0.95f, 0.7f, 0.3f),
                "Faltan actividades para niñas, niños y jóvenes.",
                "Somos muchos jóvenes y no hay nada que hacer después del colegio. Si el barrio organizara deporte, arte o cine al aire libre, nos sentiríamos parte de este lugar.");

            CrearNpc(root, 3, "rojas", "Señor Rojas", new Vector3(-8f, 0f, 14f), new Color(0.6f, 0.55f, 0.4f),
                "El espacio comunitario está deteriorado y ya casi no se usa.",
                "El salón comunitario se cayó a pedazos. Ahí hacíamos reuniones. Sin un lugar para encontrarnos, cada familia se encierra y dejamos de decidir juntos.");

            CrearNpc(root, 4, "sofia", "Sofía", new Vector3(9f, 0f, 10f), new Color(0.7f, 0.45f, 0.8f),
                "Hay poca comunicación entre vecinas y vecinos.",
                "Casi no nos hablamos. Cuando hay un problema, cada quien se queja solo. Si tuviéramos un grupo para identificar necesidades, las cosas se podrían resolver más rápido.");

            // Zona deteriorada al este
            var zona = new GameObject("ZonaDeteriorada");
            zona.transform.SetParent(root);
            Prim(zona.transform, PrimitiveType.Cube, new Vector3(28f, 0.04f, 0f), new Vector3(12f, 0.08f, 12f), new Color(0.42f, 0.36f, 0.3f), "LoteVacio");

            Reparar(zona.transform, new Vector3(25f, 0.6f, -3f), "plantar zonas verdes", new Color(0.5f, 0.38f, 0.22f), "RepararJardin");
            Reparar(zona.transform, new Vector3(28f, 1.2f, 0f), "pintar un mural comunitario", new Color(0.55f, 0.55f, 0.55f), "RepararMural");
            Reparar(zona.transform, new Vector3(31f, 0.7f, 3f), "armar un espacio de encuentro", new Color(0.4f, 0.32f, 0.28f), "RepararEncuentro");

            var restaurada = new GameObject("ZonaRestaurada");
            restaurada.transform.SetParent(root);
            restaurada.SetActive(false);
            Prim(restaurada.transform, PrimitiveType.Cube, new Vector3(28f, 0.06f, 0f), new Vector3(12f, 0.1f, 12f), new Color(0.4f, 0.8f, 0.4f), "LoteVerde");
            Banco(restaurada.transform, new Vector3(26f, 0f, 2f));
            Banco(restaurada.transform, new Vector3(30f, 0f, -2f));
            Arbol(restaurada.transform, new Vector3(24.5f, 0f, 3.5f));
            Arbol(restaurada.transform, new Vector3(31.5f, 0f, -3.2f));
            Prim(restaurada.transform, PrimitiveType.Cube, new Vector3(28f, 1.4f, 4.5f), new Vector3(4f, 2.6f, 0.25f), new Color(0.9f, 0.45f, 0.25f), "Mural");

            var extraLimpieza = new GameObject("ExtraLimpieza");
            extraLimpieza.transform.SetParent(root);
            extraLimpieza.SetActive(false);
            Prim(extraLimpieza.transform, PrimitiveType.Cube, new Vector3(-9f, 0.9f, -9f), new Vector3(0.15f, 1.6f, 1.8f), new Color(0.2f, 0.55f, 0.3f), "AvisoLimpieza");

            var extraAct = new GameObject("ExtraActividades");
            extraAct.transform.SetParent(root);
            extraAct.SetActive(false);
            Prim(extraAct.transform, PrimitiveType.Cylinder, new Vector3(-3f, 0.15f, -10f), new Vector3(3.2f, 0.15f, 3.2f), new Color(0.85f, 0.55f, 0.2f), "Cancha");
            Prim(extraAct.transform, PrimitiveType.Cube, new Vector3(4f, 0.8f, -11f), new Vector3(2.4f, 1.6f, 0.2f), new Color(0.9f, 0.2f, 0.25f), "Arco");

            var extraEsp = new GameObject("ExtraEspacios");
            extraEsp.transform.SetParent(root);
            extraEsp.SetActive(false);
            Banco(extraEsp.transform, new Vector3(-4f, 0f, 6f));
            Banco(extraEsp.transform, new Vector3(4f, 0f, 6f));
            Arbol(extraEsp.transform, new Vector3(0f, 0f, 7.5f));

            var extraGrupo = new GameObject("ExtraGrupo");
            extraGrupo.transform.SetParent(root);
            extraGrupo.SetActive(false);
            Prim(extraGrupo.transform, PrimitiveType.Cube, new Vector3(8f, 1.1f, 19f), new Vector3(0.15f, 1.8f, 2.2f), new Color(0.2f, 0.4f, 0.7f), "AvisoGrupo");

            var visuals = root.gameObject.AddComponent<VisualProgress>();
            visuals.ParqueSucio = parqueSucio;
            visuals.ParqueLimpio = parqueLimpio;
            visuals.ExtraLimpieza = extraLimpieza;
            visuals.ExtraActividades = extraAct;
            visuals.ExtraEspacios = extraEsp;
            visuals.ExtraGrupo = extraGrupo;
            visuals.ZonaDeteriorada = zona;
            visuals.ZonaRestaurada = restaurada;

            OrganizarZonas(root);
            return root;
        }

        static void OrganizarZonas(Transform root)
        {
            var viviendas = new GameObject("Zona_Viviendas").transform;
            var calles = new GameObject("Zona_Calles").transform;
            var parque = new GameObject("Zona_Parque").transform;
            var centro = new GameObject("Zona_CentroComunitario").transform;
            var verde = new GameObject("Zona_Verde").transform;
            viviendas.SetParent(root, false);
            calles.SetParent(root, false);
            parque.SetParent(root, false);
            centro.SetParent(root, false);
            verde.SetParent(root, false);

            var originales = new Transform[root.childCount - 5];
            int index = 0;
            foreach (Transform child in root)
                if (child != viviendas && child != calles && child != parque && child != centro && child != verde)
                    originales[index++] = child;

            foreach (var child in originales)
            {
                string nombre = child.name;
                Transform destino = verde;
                if (nombre == "Casa" || nombre.StartsWith("NPC_") || nombre.StartsWith("Extra")) destino = viviendas;
                else if (nombre.StartsWith("Calle") || nombre.StartsWith("Anden") || nombre.StartsWith("Caneca") || nombre.StartsWith("Residuo")) destino = calles;
                else if (nombre.StartsWith("Parque") || nombre.StartsWith("ZonaDeteriorada") || nombre.StartsWith("ZonaRestaurada")) destino = parque;
                else if (nombre.Contains("Centro") || nombre.Contains("Asamblea") || nombre.StartsWith("Tablero")) destino = centro;
                child.SetParent(destino, true);
            }
        }

        static void Reparar(Transform parent, Vector3 pos, string accion, Color color, string nombre)
        {
            var go = new GameObject(nombre);
            go.transform.SetParent(parent);
            go.transform.position = pos;
            var trigger = go.AddComponent<BoxCollider>();
            trigger.size = new Vector3(2.4f, 2.2f, 2.4f);
            trigger.isTrigger = true;

            if (nombre == "RepararJardin")
            {
                Prim(go.transform, PrimitiveType.Cylinder, pos + new Vector3(0f, -0.38f, 0f), new Vector3(1.5f, 0.12f, 1.5f), new Color(0.35f, 0.24f, 0.15f), "Alcorque");
                var brote = Prim(go.transform, PrimitiveType.Cylinder, pos + new Vector3(0f, 0.25f, 0f), new Vector3(0.12f, 0.55f, 0.12f), new Color(0.25f, 0.48f, 0.2f), "Brote");
                brote.transform.rotation = Quaternion.Euler(0f, 0f, -10f);
                Prim(go.transform, PrimitiveType.Sphere, pos + new Vector3(-0.36f, 0.46f, 0f), new Vector3(0.65f, 0.18f, 0.32f), new Color(0.34f, 0.68f, 0.3f), "HojaIzquierda");
                Prim(go.transform, PrimitiveType.Sphere, pos + new Vector3(0.36f, 0.57f, 0f), new Vector3(0.65f, 0.18f, 0.32f), new Color(0.42f, 0.76f, 0.34f), "HojaDerecha");
            }
            else if (nombre == "RepararMural")
            {
                Prim(go.transform, PrimitiveType.Cube, pos, new Vector3(2.1f, 1.9f, 0.28f), new Color(0.8f, 0.78f, 0.68f), "PanelMural");
                Prim(go.transform, PrimitiveType.Cube, pos + new Vector3(0f, 0f, -0.18f), new Vector3(1.65f, 1.35f, 0.06f), new Color(0.12f, 0.58f, 0.62f), "FondoMural");
                Prim(go.transform, PrimitiveType.Sphere, pos + new Vector3(-0.38f, 0.08f, -0.24f), new Vector3(0.55f, 0.55f, 0.08f), new Color(0.98f, 0.72f, 0.28f), "SolMural");
                Prim(go.transform, PrimitiveType.Cube, pos + new Vector3(0.27f, -0.38f, -0.24f), new Vector3(0.22f, 0.8f, 0.08f), new Color(0.28f, 0.38f, 0.27f), "ArbolMural");
                Prim(go.transform, PrimitiveType.Sphere, pos + new Vector3(0.27f, 0.02f, -0.25f), new Vector3(0.7f, 0.5f, 0.08f), new Color(0.4f, 0.73f, 0.38f), "CopaMural");
            }
            else
            {
                Banco(go.transform, pos + new Vector3(0f, -0.3f, 0f));
                Prim(go.transform, PrimitiveType.Cube, pos + new Vector3(-0.58f, 0.22f, -0.18f), new Vector3(0.14f, 0.8f, 0.14f), color, "SoporteBanco");
                Prim(go.transform, PrimitiveType.Cube, pos + new Vector3(0.58f, 0.22f, -0.18f), new Vector3(0.14f, 0.8f, 0.14f), color, "SoporteBanco");
            }
            QuitarCollidersDeDecoracion(go.transform);
            var spot = go.AddComponent<RepairSpot>();
            spot.Accion = accion;
        }

        static void Caneca(Transform parent, Vector3 pos, TrashKind tipo, Color color, string nombre)
        {
            var go = new GameObject(nombre);
            go.transform.SetParent(parent);
            go.transform.position = pos;
            Prim(go.transform, PrimitiveType.Cylinder, pos, new Vector3(0.9f, 0.55f, 0.9f), color, "CuerpoCaneca");
            Prim(go.transform, PrimitiveType.Cylinder, pos + new Vector3(0f, 0.52f, 0f), new Vector3(0.98f, 0.08f, 0.98f), new Color(0.12f, 0.16f, 0.19f), "TapaCaneca");
            Prim(go.transform, PrimitiveType.Cylinder, pos + new Vector3(0f, 0.61f, 0f), new Vector3(0.34f, 0.025f, 0.34f), new Color(0.08f, 0.1f, 0.12f), "BocaCaneca");
            Prim(go.transform, PrimitiveType.Cylinder, pos + new Vector3(0f, -0.18f, 0f), new Vector3(0.92f, 0.07f, 0.92f), new Color(0.92f, 0.94f, 0.9f), "BandaIdentificadora");
            // Aros laterales: hacen que el código de color se lea desde lejos.
            Prim(go.transform, PrimitiveType.Cube, pos + new Vector3(0f, 0.12f, 0.47f), new Vector3(0.52f, 0.1f, 0.035f), color, "MarcaTipo");
            // Símbolo en relieve para reconocer cada caneca incluso sin depender del color.
            Prim(go.transform, PrimitiveType.Cube, pos + new Vector3(0f, -0.12f, 0.485f), new Vector3(0.38f, 0.34f, 0.035f), new Color(0.93f, 0.94f, 0.88f), "PlacaIcono");
            if (tipo == TrashKind.Reciclable)
            {
                Prim(go.transform, PrimitiveType.Cylinder, pos + new Vector3(0f, -0.1f, 0.53f), new Vector3(0.13f, 0.11f, 0.02f), color, "IconoBotella");
                Prim(go.transform, PrimitiveType.Cylinder, pos + new Vector3(0f, 0.01f, 0.53f), new Vector3(0.07f, 0.035f, 0.02f), color, "IconoCuello");
            }
            else
            {
                var hojaA = Prim(go.transform, PrimitiveType.Sphere, pos + new Vector3(-0.07f, -0.1f, 0.53f), new Vector3(0.18f, 0.09f, 0.035f), color, "IconoHoja");
                hojaA.transform.rotation = Quaternion.Euler(0f, 0f, 30f);
                var hojaB = Prim(go.transform, PrimitiveType.Sphere, pos + new Vector3(0.07f, -0.1f, 0.53f), new Vector3(0.18f, 0.09f, 0.035f), color, "IconoHoja");
                hojaB.transform.rotation = Quaternion.Euler(0f, 0f, -30f);
            }
            QuitarCollidersDeDecoracion(go.transform);
            var hitbox = go.AddComponent<CapsuleCollider>();
            hitbox.center = new Vector3(0f, 0f, 0f);
            hitbox.height = 1.2f;
            hitbox.radius = 0.48f;
            hitbox.isTrigger = true;
            var bin = go.AddComponent<TrashBin>();
            bin.TipoAceptado = tipo;
        }

        static void CrearNpc(Transform parent, int modelIndex, string id, string nombre, Vector3 pos, Color color, string problema, string dialogo)
        {
            var npc = new GameObject("NPC_" + nombre);
            npc.transform.SetParent(parent);
            npc.transform.position = pos;

            var modeloVecino = VisualSettings != null ? VisualSettings.GetResidentModel(modelIndex) : null;
            if (modeloVecino != null)
            {
                var modelo = Object.Instantiate(modeloVecino, npc.transform);
                modelo.name = "ModeloVisual";
                modelo.transform.localPosition = VisualSettings.residentModelOffset;
                modelo.transform.localRotation = Quaternion.Euler(VisualSettings.residentModelRotation);
                modelo.transform.localScale = VisualSettings.residentModelScale;
            }
            else
            {
            // Silueta completa con ropa, brazos y zapatos para distinguir mejor a cada vecino.
            Color chaqueta = ColorChaqueta(id);
            CrearTorso(npc.transform, color);
            VisualPrim(npc.transform, PrimitiveType.Cube, pos + new Vector3(-0.2f, 1.2f, 0.27f), new Vector3(0.22f, 0.72f, 0.12f), chaqueta, "SolapaChaqueta_I");
            VisualPrim(npc.transform, PrimitiveType.Cube, pos + new Vector3(0.2f, 1.2f, 0.27f), new Vector3(0.22f, 0.72f, 0.12f), chaqueta, "SolapaChaqueta_D");
            VisualPrim(npc.transform, PrimitiveType.Cube, pos + new Vector3(0f, 1.2f, 0.337f), new Vector3(0.025f, 0.62f, 0.02f), new Color(0.91f, 0.8f, 0.58f), "Cremallera");
            var pantalones = id == "valentina" || id == "sofia" ? new Color(0.2f, 0.27f, 0.4f) : new Color(0.24f, 0.3f, 0.32f);
            VisualPrim(npc.transform, PrimitiveType.Capsule, pos + new Vector3(-0.19f, 0.34f, 0f), new Vector3(0.2f, 0.52f, 0.22f), pantalones, "PiernaIzquierda");
            VisualPrim(npc.transform, PrimitiveType.Capsule, pos + new Vector3(0.19f, 0.34f, 0f), new Vector3(0.2f, 0.52f, 0.22f), pantalones, "PiernaDerecha");
            VisualPrim(npc.transform, PrimitiveType.Cube, pos + new Vector3(-0.19f, 0.08f, 0.08f), new Vector3(0.28f, 0.14f, 0.4f), new Color(0.13f, 0.15f, 0.17f), "ZapatoIzquierdo");
            VisualPrim(npc.transform, PrimitiveType.Cube, pos + new Vector3(0.19f, 0.08f, 0.08f), new Vector3(0.28f, 0.14f, 0.4f), new Color(0.13f, 0.15f, 0.17f), "ZapatoDerecho");
            VisualPrim(npc.transform, PrimitiveType.Capsule, pos + new Vector3(-0.43f, 1.1f, 0f), new Vector3(0.18f, 0.48f, 0.2f), chaqueta, "MangaChaquetaIzquierda");
            VisualPrim(npc.transform, PrimitiveType.Capsule, pos + new Vector3(0.43f, 1.1f, 0f), new Vector3(0.18f, 0.48f, 0.2f), chaqueta, "MangaChaquetaDerecha");
            VisualPrim(npc.transform, PrimitiveType.Cube, pos + new Vector3(-0.43f, 0.72f, 0.02f), new Vector3(0.18f, 0.1f, 0.2f), chaqueta, "PuñoChaquetaIzquierdo");
            VisualPrim(npc.transform, PrimitiveType.Cube, pos + new Vector3(0.43f, 0.72f, 0.02f), new Vector3(0.18f, 0.1f, 0.2f), chaqueta, "PuñoChaquetaDerecho");

            Color piel = id == "luis" || id == "rojas" ? new Color(0.72f, 0.48f, 0.32f) : new Color(0.88f, 0.65f, 0.48f);
            VisualPrim(npc.transform, PrimitiveType.Sphere, pos + new Vector3(-0.43f, 0.58f, 0.02f), new Vector3(0.15f, 0.18f, 0.13f), piel, "ManoIzquierda");
            VisualPrim(npc.transform, PrimitiveType.Sphere, pos + new Vector3(0.43f, 0.58f, 0.02f), new Vector3(0.15f, 0.18f, 0.13f), piel, "ManoDerecha");
            VisualPrim(npc.transform, PrimitiveType.Cylinder, pos + new Vector3(0f, 1.72f, 0f), new Vector3(0.16f, 0.18f, 0.16f), piel, "Cuello");
            VisualPrim(npc.transform, PrimitiveType.Sphere, pos + new Vector3(0f, 2.08f, 0f), new Vector3(0.52f, 0.62f, 0.48f), piel, "Cabeza");
            Color pelo = id == "carmen" || id == "rojas" ? new Color(0.28f, 0.22f, 0.18f) : new Color(0.12f, 0.1f, 0.09f);
            VisualPrim(npc.transform, PrimitiveType.Sphere, pos + new Vector3(0f, 2.35f, -0.03f), new Vector3(0.55f, 0.3f, 0.5f), pelo, "Cabello");
            if (id == "carmen")
                VisualPrim(npc.transform, PrimitiveType.Sphere, pos + new Vector3(0f, 2.48f, -0.32f), Vector3.one * 0.2f, pelo, "Moño");
            else if (id == "valentina" || id == "sofia")
                VisualPrim(npc.transform, PrimitiveType.Sphere, pos + new Vector3(0.34f, 2.17f, -0.05f), new Vector3(0.18f, 0.38f, 0.2f), pelo, "Coleta");
            else if (id == "luis")
            {
                VisualPrim(npc.transform, PrimitiveType.Cylinder, pos + new Vector3(0f, 2.53f, 0f), new Vector3(0.58f, 0.08f, 0.58f), new Color(0.83f, 0.68f, 0.4f), "Sombrero");
                VisualPrim(npc.transform, PrimitiveType.Cylinder, pos + new Vector3(0f, 2.66f, 0f), new Vector3(0.37f, 0.18f, 0.37f), new Color(0.83f, 0.68f, 0.4f), "CopaSombrero");
            }
            else if (id == "rojas")
                VisualPrim(npc.transform, PrimitiveType.Cylinder, pos + new Vector3(0f, 2.52f, 0f), new Vector3(0.55f, 0.07f, 0.55f), new Color(0.2f, 0.3f, 0.42f), "Gorra");

            // Ojos y sonrisa orientados hacia la calle.
            foreach (int lado in new[] { -1, 1 })
            {
                float x = lado * 0.13f;
                VisualPrim(npc.transform, PrimitiveType.Sphere, pos + new Vector3(x, 2.12f, 0.46f), new Vector3(0.085f, 0.09f, 0.04f), new Color(0.96f, 0.93f, 0.84f), lado < 0 ? "OjoIzquierdo" : "OjoDerecho");
                VisualPrim(npc.transform, PrimitiveType.Sphere, pos + new Vector3(x, 2.12f, 0.49f), Vector3.one * 0.037f, new Color(0.17f, 0.5f, 0.7f), lado < 0 ? "IrisIzquierdo" : "IrisDerecho");
                VisualPrim(npc.transform, PrimitiveType.Sphere, pos + new Vector3(x, 2.12f, 0.515f), Vector3.one * 0.019f, new Color(0.1f, 0.11f, 0.13f), lado < 0 ? "PupilaIzquierda" : "PupilaDerecha");
            }
            VisualPrim(npc.transform, PrimitiveType.Sphere, pos + new Vector3(0f, 2.035f, 0.47f), new Vector3(0.1f, 0.13f, 0.13f), piel, "Nariz");
            VisualPrim(npc.transform, PrimitiveType.Sphere, pos + new Vector3(-0.28f, 2.08f, 0f), new Vector3(0.1f, 0.16f, 0.12f), piel, "OrejaIzquierda");
            VisualPrim(npc.transform, PrimitiveType.Sphere, pos + new Vector3(0.28f, 2.08f, 0f), new Vector3(0.1f, 0.16f, 0.12f), piel, "OrejaDerecha");
            VisualPrim(npc.transform, PrimitiveType.Cube, pos + new Vector3(-0.13f, 2.21f, 0.48f), new Vector3(0.13f, 0.035f, 0.035f), pelo, "CejaIzquierda");
            VisualPrim(npc.transform, PrimitiveType.Cube, pos + new Vector3(0.13f, 2.21f, 0.48f), new Vector3(0.13f, 0.035f, 0.035f), pelo, "CejaDerecha");
            var sonrisa = VisualPrim(npc.transform, PrimitiveType.Cube, pos + new Vector3(0f, 1.94f, 0.49f), new Vector3(0.14f, 0.025f, 0.02f), new Color(0.42f, 0.18f, 0.16f), "Sonrisa");
            sonrisa.transform.rotation = Quaternion.Euler(0f, 0f, -5f);
            if (id == "luis" || id == "rojas")
                VisualPrim(npc.transform, PrimitiveType.Sphere, pos + new Vector3(0f, 1.84f, 0.36f), new Vector3(0.3f, 0.1f, 0.08f), new Color(0.32f, 0.24f, 0.19f), "Barba");
            // Cuello de camisa y botones aportan pliegues y detalles a la ropa.
            VisualPrim(npc.transform, PrimitiveType.Cube, pos + new Vector3(0f, 1.66f, 0.27f), new Vector3(0.24f, 0.1f, 0.08f), new Color(0.91f, 0.88f, 0.78f), "CuelloCamisa");
            for (int boton = 0; boton < 3; boton++)
                VisualPrim(npc.transform, PrimitiveType.Sphere, pos + new Vector3(0f, 1.25f - boton * 0.17f, 0.27f), Vector3.one * 0.035f, new Color(0.9f, 0.83f, 0.62f), "Boton");
            }

            QuitarCollidersDeDecoracion(npc.transform);
            var trigger = npc.AddComponent<CapsuleCollider>();
            trigger.isTrigger = true;
            trigger.center = new Vector3(0f, 1f, 0f);
            trigger.height = 2.2f;
            trigger.radius = 0.7f;

            var comp = npc.AddComponent<NPCCharacter>();
            comp.Id = id;
            comp.Nombre = nombre;
            comp.Problema = problema;
            comp.Dialogo = dialogo;

            var etiqueta = new GameObject("Etiqueta");
            etiqueta.transform.SetParent(npc.transform);
            etiqueta.transform.localPosition = new Vector3(0f, 2.9f, 0f);
            var canvas = etiqueta.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.worldCamera = Camera.main;
            var rt = canvas.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(170f, 28f);
            rt.localScale = Vector3.one * 0.02f;
            var fondo = etiqueta.AddComponent<UnityEngine.UI.Image>();
            fondo.color = new Color(0.08f, 0.15f, 0.19f, 0.92f);
            fondo.raycastTarget = false;
            var textoGo = new GameObject("TextoNombre", typeof(RectTransform), typeof(UnityEngine.UI.Text));
            textoGo.transform.SetParent(etiqueta.transform, false);
            var textoRect = textoGo.GetComponent<RectTransform>();
            textoRect.sizeDelta = rt.sizeDelta;
            textoRect.localScale = Vector3.one;
            var texto = textoGo.GetComponent<UnityEngine.UI.Text>();
            texto.font = UiFactory.Fuente;
            texto.text = nombre;
            texto.alignment = TextAnchor.MiddleCenter;
            texto.color = Color.white;
            texto.fontSize = 18;
            texto.horizontalOverflow = HorizontalWrapMode.Overflow;
            etiqueta.AddComponent<Billboard>();
        }

        static Color ColorChaqueta(string id)
        {
            if (id == "carmen") return new Color(0.68f, 0.12f, 0.2f);
            if (id == "luis") return new Color(0.16f, 0.34f, 0.52f);
            if (id == "valentina") return new Color(0.74f, 0.12f, 0.16f);
            if (id == "rojas") return new Color(0.48f, 0.3f, 0.15f);
            if (id == "sofia") return new Color(0.43f, 0.2f, 0.55f);
            return new Color(0.68f, 0.12f, 0.2f);
        }

        static void Casa(Transform parent, Vector3 pos, Color pared)
        {
            var go = new GameObject("Casa");
            go.transform.SetParent(parent);
            go.transform.position = pos;
            Prim(go.transform, PrimitiveType.Cube, pos + new Vector3(0f, 1.4f, 0f), new Vector3(5.2f, 2.8f, 5.2f), pared, "Muro");
            Prim(go.transform, PrimitiveType.Cube, pos + new Vector3(0f, 0.16f, 0f), new Vector3(5.45f, 0.28f, 5.45f), new Color(0.82f, 0.78f, 0.68f), "Zocalo");
            // Techo a dos aguas, con aleros y fascia visible.
            var techoA = Prim(go.transform, PrimitiveType.Cube, pos + new Vector3(0f, 3.15f, -1.35f), new Vector3(5.9f, 0.28f, 3.25f), Techo, "TechoTrasero");
            techoA.transform.rotation = Quaternion.Euler(-28f, 0f, 0f);
            var techoB = Prim(go.transform, PrimitiveType.Cube, pos + new Vector3(0f, 3.15f, 1.35f), new Vector3(5.9f, 0.28f, 3.25f), Techo, "TechoFrontal");
            techoB.transform.rotation = Quaternion.Euler(28f, 0f, 0f);
            Prim(go.transform, PrimitiveType.Cube, pos + new Vector3(0f, 3.35f, 2.58f), new Vector3(5.8f, 0.16f, 0.16f), new Color(0.93f, 0.87f, 0.72f), "Alero");
            Color marco = new Color(0.92f, 0.86f, 0.68f);
            for (int lado = -1; lado <= 1; lado += 2)
            {
                float x = lado * 1.55f;
                Prim(go.transform, PrimitiveType.Cube, pos + new Vector3(x, 1.85f, 2.64f), new Vector3(1.05f, 0.92f, 0.12f), marco, "MarcoVentana");
                Prim(go.transform, PrimitiveType.Cube, pos + new Vector3(x, 1.85f, 2.72f), new Vector3(0.79f, 0.66f, 0.06f), new Color(0.32f, 0.68f, 0.78f), "VidrioVentana");
                Prim(go.transform, PrimitiveType.Cube, pos + new Vector3(x, 1.85f, 2.77f), new Vector3(0.06f, 0.72f, 0.04f), marco, "DivisorVentana");
                Prim(go.transform, PrimitiveType.Cube, pos + new Vector3(x, 1.85f, 2.77f), new Vector3(0.85f, 0.06f, 0.04f), marco, "DivisorVentana");
            }
            Prim(go.transform, PrimitiveType.Cube, pos + new Vector3(0f, 0.78f, 2.66f), new Vector3(1.05f, 1.55f, 0.17f), new Color(0.35f, 0.22f, 0.12f), "Puerta");
            Prim(go.transform, PrimitiveType.Sphere, pos + new Vector3(0.36f, 0.78f, 2.77f), Vector3.one * 0.09f, new Color(0.92f, 0.72f, 0.28f), "Perilla");
            Prim(go.transform, PrimitiveType.Cube, pos + new Vector3(0f, 2.94f, 2.66f), new Vector3(1.4f, 0.3f, 0.12f), new Color(0.95f, 0.91f, 0.8f), "Dintel");
        }

        static void Arbol(Transform parent, Vector3 pos)
        {
            var arbol = new GameObject("Arbol");
            arbol.transform.SetParent(parent, false);
            arbol.transform.position = pos;
            var tronco = Prim(arbol.transform, PrimitiveType.Cylinder, pos + new Vector3(0f, 0.8f, 0f), new Vector3(0.35f, 0.8f, 0.35f), Tronco, "Tronco");
            QuitarCollider(tronco);
            var copa = Prim(arbol.transform, PrimitiveType.Sphere, pos + new Vector3(0f, 2.1f, 0f), new Vector3(2.1f, 2.1f, 2.1f), Copa, "Copa");
            QuitarCollider(copa);
            Color hojasClaras = new Color(0.3f, 0.63f, 0.31f);
            VisualPrim(arbol.transform, PrimitiveType.Sphere, pos + new Vector3(-0.68f, 2.05f, -0.48f), new Vector3(1.15f, 1.2f, 1.1f), hojasClaras, "FollajeIzquierdo");
            VisualPrim(arbol.transform, PrimitiveType.Sphere, pos + new Vector3(0.65f, 2.28f, 0.45f), new Vector3(1.2f, 1.1f, 1.15f), Copa, "FollajeDerecho");
            VisualPrim(arbol.transform, PrimitiveType.Sphere, pos + new Vector3(0.05f, 2.75f, -0.15f), new Vector3(1.25f, 1.1f, 1.15f), new Color(0.17f, 0.48f, 0.25f), "FollajeSuperior");
        }

        static void Farola(Transform parent, Vector3 pos)
        {
            var farola = new GameObject("Farola");
            farola.transform.SetParent(parent, false);
            farola.transform.position = pos;
            Color metal = new Color(0.18f, 0.24f, 0.27f);
            VisualPrim(farola.transform, PrimitiveType.Cylinder, pos + new Vector3(0f, 0.12f, 0f), new Vector3(0.48f, 0.12f, 0.48f), metal, "Base");
            VisualPrim(farola.transform, PrimitiveType.Cylinder, pos + new Vector3(0f, 1.65f, 0f), new Vector3(0.12f, 1.55f, 0.12f), metal, "Poste");
            VisualPrim(farola.transform, PrimitiveType.Cube, pos + new Vector3(0.48f, 3.1f, 0f), new Vector3(1f, 0.1f, 0.1f), metal, "Brazo");
            VisualPrim(farola.transform, PrimitiveType.Cube, pos + new Vector3(0.86f, 3f, 0f), new Vector3(0.42f, 0.12f, 0.3f), new Color(0.96f, 0.78f, 0.43f), "Luz");
        }

        static void QuitarCollider(GameObject go)
        {
            var col = go.GetComponent<Collider>();
            if (col == null)
                return;
            col.enabled = false;
            if (Application.isPlaying)
                Object.Destroy(col);
            else
                Object.DestroyImmediate(col);
        }

        static void QuitarCollidersDeDecoracion(Transform root)
        {
            foreach (var collider in root.GetComponentsInChildren<Collider>())
            {
                if (collider.transform == root)
                    continue;
                collider.enabled = false;
                if (Application.isPlaying)
                    Object.Destroy(collider);
                else
                    Object.DestroyImmediate(collider);
            }
        }

        static GameObject CrearTorso(Transform parent, Color color)
        {
            // Volumen anatómico con cintura y hombros, en vez de una cápsula uniforme.
            Vector2[] perfil =
            {
                new Vector2(0.48f, 0.24f),
                new Vector2(0.62f, 0.29f),
                new Vector2(0.92f, 0.32f),
                new Vector2(1.2f, 0.4f),
                new Vector2(1.45f, 0.37f),
                new Vector2(1.62f, 0.18f)
            };
            const int segmentos = 12;
            var vertices = new Vector3[perfil.Length * segmentos + 2];
            var uv = new Vector2[vertices.Length];
            for (int fila = 0; fila < perfil.Length; fila++)
            {
                float y = perfil[fila].x;
                float radioX = perfil[fila].y;
                float radioZ = radioX * 0.58f;
                for (int segmento = 0; segmento < segmentos; segmento++)
                {
                    float angulo = segmento * Mathf.PI * 2f / segmentos;
                    int index = fila * segmentos + segmento;
                    vertices[index] = new Vector3(Mathf.Sin(angulo) * radioX, y, Mathf.Cos(angulo) * radioZ);
                    uv[index] = new Vector2((float)segmento / segmentos, (y - 0.48f) / 1.14f);
                }
            }
            int centroInferior = perfil.Length * segmentos;
            int centroSuperior = centroInferior + 1;
            vertices[centroInferior] = new Vector3(0f, perfil[0].x, 0f);
            vertices[centroSuperior] = new Vector3(0f, perfil[perfil.Length - 1].x, 0f);

            var triangulos = new int[(perfil.Length - 1) * segmentos * 6 + segmentos * 6];
            int t = 0;
            for (int fila = 0; fila < perfil.Length - 1; fila++)
            {
                for (int segmento = 0; segmento < segmentos; segmento++)
                {
                    int siguiente = (segmento + 1) % segmentos;
                    int bajo = fila * segmentos + segmento;
                    int bajoSiguiente = fila * segmentos + siguiente;
                    int alto = (fila + 1) * segmentos + segmento;
                    int altoSiguiente = (fila + 1) * segmentos + siguiente;
                    triangulos[t++] = bajo;
                    triangulos[t++] = bajoSiguiente;
                    triangulos[t++] = altoSiguiente;
                    triangulos[t++] = bajo;
                    triangulos[t++] = altoSiguiente;
                    triangulos[t++] = alto;
                }
            }
            for (int segmento = 0; segmento < segmentos; segmento++)
            {
                int siguiente = (segmento + 1) % segmentos;
                triangulos[t++] = centroInferior;
                triangulos[t++] = siguiente;
                triangulos[t++] = segmento;

                int superiorActual = (perfil.Length - 1) * segmentos + segmento;
                int superiorSiguiente = (perfil.Length - 1) * segmentos + siguiente;
                triangulos[t++] = centroSuperior;
                triangulos[t++] = superiorActual;
                triangulos[t++] = superiorSiguiente;
            }

            var mesh = new Mesh { name = "TorsoVecino" };
            mesh.vertices = vertices;
            mesh.uv = uv;
            mesh.triangles = triangulos;
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();

            var cuerpo = new GameObject("Ropa");
            cuerpo.transform.SetParent(parent, false);
            cuerpo.AddComponent<MeshFilter>().sharedMesh = mesh;
            cuerpo.AddComponent<MeshRenderer>().sharedMaterial = WorldPalette.Color(color);
            return cuerpo;
        }

        static void SinCollider(GameObject go) => QuitarCollider(go);

        static GameObject VisualPrim(Transform parent, PrimitiveType tipo, Vector3 pos, Vector3 escala, Color color, string nombre)
        {
            var go = Prim(parent, tipo, pos, escala, color, nombre);
            QuitarCollider(go);
            return go;
        }

        static void Banco(Transform parent, Vector3 pos)
        {
            var banco = new GameObject("Banca");
            banco.transform.SetParent(parent, false);
            banco.transform.position = pos;
            Color madera = new Color(0.53f, 0.31f, 0.16f);
            for (int i = -1; i <= 1; i++)
                VisualPrim(banco.transform, PrimitiveType.Cube, pos + new Vector3(0f, 0.49f, i * 0.17f), new Vector3(1.8f, 0.12f, 0.13f), madera, "TablaBanco");
            VisualPrim(banco.transform, PrimitiveType.Cube, pos + new Vector3(0f, 0.92f, -0.22f), new Vector3(1.8f, 0.12f, 0.13f), madera, "RespaldoBanco");
            VisualPrim(banco.transform, PrimitiveType.Cube, pos + new Vector3(0f, 0.7f, -0.22f), new Vector3(1.8f, 0.38f, 0.1f), madera, "RespaldoBanco");
            for (int i = -1; i <= 1; i += 2)
            {
                VisualPrim(banco.transform, PrimitiveType.Cube, pos + new Vector3(i * 0.62f, 0.24f, -0.1f), new Vector3(0.12f, 0.48f, 0.48f), new Color(0.2f, 0.24f, 0.25f), "PataBanco");
                VisualPrim(banco.transform, PrimitiveType.Cube, pos + new Vector3(i * 0.62f, 0.66f, -0.22f), new Vector3(0.12f, 0.7f, 0.12f), new Color(0.2f, 0.24f, 0.25f), "SoporteRespaldo");
            }
        }

        static GameObject Prim(Transform parent, PrimitiveType tipo, Vector3 pos, Vector3 escala, Color color, string nombre)
        {
            var go = GameObject.CreatePrimitive(tipo);
            go.name = nombre;
            go.transform.SetParent(parent);
            go.transform.position = pos;
            go.transform.localScale = escala;
            go.GetComponent<Renderer>().sharedMaterial = WorldPalette.Color(color);
            return go;
        }
    }
}
