$(document).ready(function () {
    let idIntervalBolilla;
    let idIntervalContador;
    let numeroBolilla = 0;
    let bolillas = [1, 2, 3, 4, 5, 6, 7, 8, 9,
        10, 11, 12, 13, 14, 15, 16, 17, 18, 19,
        20, 21, 22, 23, 24, 25, 26, 27, 28, 29,
        30, 31, 32, 33, 34, 35, 36, 37, 38, 39,
        40, 41, 42, 43, 44, 45, 46, 47, 48, 49,
        50, 51, 52, 53, 54, 55, 56, 57, 58, 59,
        60, 61, 62, 63, 64, 65, 66, 67, 68, 69,
        70, 71, 72, 73, 74, 75, 76, 77, 78, 79,
        80, 81, 82, 83, 84, 85, 86, 87, 88, 89,
        90, 91, 92, 93, 94, 95, 96, 97, 98, 99];

    function aleatorizarArray(array) {
        let obj = [];

        array.forEach(element => {
            obj.push({ e: element, d: Math.random() });
        });

        obj.sort(function (a, b) {
            if (a.d < b.d)
                return -1;
        });

        return obj.map(x => x.e);
    }

    bolillas = aleatorizarArray(bolillas);

    function hayGanador() {
        let res = [];
        for (let i = 1; i < 5; i++) {
            if ($(`#C${i} .number:not('.marked')`).length == 0)
                res.push(i);
        }
        return res;
    }

    function marcarCarton(numeroBolilla) {
        $(`.${numeroBolilla}`).addClass("marked");
    }

    function obtenerBolilla() {
        let ganador = hayGanador();

        if (ganador.length > 0) {
            let g = JSON.stringify(ganador);
            $("#Ganador").html(g);
            numeroBolilla = 100;

            let obj = {}
            obj.ganadores = ganador;
            obj.carton1 = ganador.includes(1) ? $("#C1").attr("data-carton") : null;
            obj.carton2 = ganador.includes(2) ? $("#C2").attr("data-carton") : null;
            obj.carton3 = ganador.includes(3) ? $("#C3").attr("data-carton") : null;
            obj.carton4 = ganador.includes(4) ? $("#C4").attr("data-carton") : null;
            obj.bolillero = bolillas;

            $.ajax({
                method: "POST",
                data: obj,
                url: "/Home/GuardarDatos"
            })
                .done(function (res) {
                    alert("Los datos se guardaron correctamente");
                })
                .fail(function (jqHR, textStatus, errorThrown) {
                    alert("Hubo un inconveniente al intentar guardar los datos");
                });
        }

        if (numeroBolilla < 99) {
            let bolilla = bolillas[numeroBolilla];
            numeroBolilla++;
            $("#Bolilla").html(bolilla);
            marcarCarton(bolilla);
        }
        else {
            clearInterval(idIntervalBolilla);
            clearInterval(idIntervalContador);
            if ($("#Switch").prop("checked"))
                toggleBoton();
        }
    }

    function toggleBoton() {
        if ($("#Lanzar").hasClass("btn-primary")) {
            $("#Lanzar").html("[Detener]")
                .removeClass("btn-primary")
                .addClass("btn-danger");
            $("#Counter").html("5");
            $("#Switch").attr("disabled", "disabled");
        }
        else {
            $("#Lanzar").html("[Lanzar Bolilla]")
                .removeClass("btn-danger")
                .addClass("btn-primary");
            $("#Switch").removeAttr("disabled");
        }
    }

    function contador() {
        let number = parseInt($("#Counter").html());

        if (number == 0) {
            number = 4;
        }
        else {
            number--;
        }

        $("#Counter").html(number);
    }

    $("#Switch").on("click", function () {
        if ($("#Switch").prop("checked"))
            $("#Counter").removeClass("d-none");
        else
            $("#Counter").addClass("d-none");
    });

    $("#Lanzar").on("click", function () {
        if ($("#Switch").prop("checked")) {
            if ($(this).hasClass("btn-primary")) {
                idIntervalBolilla = setInterval(obtenerBolilla, 5000);
                idIntervalContador = setInterval(contador, 1000);
            }
            else {
                clearInterval(idIntervalBolilla);
                clearInterval(idIntervalContador);
            }
            toggleBoton();
        }
        else {
            obtenerBolilla();
        }
    });

    function setCarton(carton, numero) {
        $(`#C${numero}`).attr("data-carton", JSON.stringify(carton));

        for (let c = 0; c < 9; c++) {
            for (let f = 0; f < 3; f++) {
                let value = carton[f][c];
                let div = $(`#C${numero} > div:nth-child(${c + 1}) > div:nth-child(${f + 1})`);

                if (value != "__")
                    $(div).html(value).addClass(value).addClass("number");
                else
                    $(div).html("").addClass("empty");
            }
        }
    }

    $.ajax({
        method: "GET",
        url: "/Home/ObtenerCartones"
    })
        .done(function (res) {
            /* (4) [Array(3), Array(3), Array(3), Array(3)]
                    0:Array(3)
                        0: (9) ['1', '__', '__', '30', '__', '51', '__', '73', '80']
                        1: (9) ['__', '15', '__', '__', '44', '57', '63', '77', '__']
                        2: (9) ['8', '__', '25', '__', '46', '__', '68', '__', '86']
                        length: 3
                        [[Prototype]]: Array(0)
                    1: (3) [Array(9), Array(9), Array(9)]
                    2: (3) [Array(9), Array(9), Array(9)]
                    3: (3) [Array(9), Array(9), Array(9)]
            */
            let cartones = JSON.parse(res);
            for (let i = 0; i < 4; i++) {
                setCarton(cartones[i], i + 1);
            }
        })
        .fail(function (jqHR, textStatus, errorThrown) {
            alert("Hubo un inconveniente al intentar traer los cartones. Intente nuevamente");
        });
});