﻿!function (n) { "use strict"; n.fn.novacancyID = void 0 === n.fn.novacancyID ? 0 : n.fn.novacancyID, n.fn.novacancy = function (i) { return n.each(this, function (o, t) { new function (i, o) { var t, a, l, e, c, r = this, s = n(i); if (this.repeat = function () { return !!s[0].novacancy || (s[0].novacancy = !0, !1) }, this.setAppearance = function () { var i = ++n.fn.novacancyID, o = '[novacancy-id="' + i + '"]'; s.attr("novacancy-id", i), r.addCSS(o) }, this.addCSS = function (i) { var o = r.css(i), t = n("<style>" + o + "</style>"); n("body").append(t) }, this.css = function (n) { var i = "", o = "", a = ""; null !== t.color && (i += "color: " + t.color + ";", o += "color: " + t.color + "; opacity: 0.3;"), null !== t.glow && (i += a += "text-shadow: " + t.glow.toString() + ";"); var l = ""; return l += n + " .novacancy." + t.classOn + " { " + i + " }\n", l += n + " .novacancy." + t.classOff + " { " + o + " }\n" }, this.rand = function (n, i) { return Math.floor(Math.random() * (i - n + 1) + n) }, this.blink = function (n) { r.off(n), n[0].blinking = !0, setTimeout(function () { r.on(n), r.reblink(n) }, r.rand(t.blinkMin, t.blinkMax)) }, this.reblink = function (n) { setTimeout(function () { r.rand(1, 100) <= t.reblinkProbability ? r.blink(n) : n[0].blinking = !1 }, r.rand(t.blinkMin, t.blinkMax)) }, this.on = function (n) { n.removeClass(t.classOff).addClass(t.classOn) }, this.off = function (n) { n.removeClass(t.classOn).addClass(t.classOff) }, this.buildHTML = function () { var n = this.htmlString; s.html(n) }, this.htmlString = function () { var i = ""; return n.each(s.contents(), function (o, a) { if (3 == a.nodeType) { var l = a.nodeValue.split(""); n.each(l, function (n, o) { i += "<" + t.element + ' class="novacancy ' + t.classOn + '">' + o + "</" + t.element + ">" }) } else i += a.outerHTML }), i }, this.newArray = function () { var i, o = e.length, a = r.randomArray(o), l = t.off, c = t.blink; return l = Math.min(l, o), l = Math.max(0, l), i = a.splice(0, l), n.each(i, function (n, i) { r.off(e.eq(i)) }), c = 0 === c ? o : c, c = Math.min(c, o - l), c = Math.max(0, c), a.splice(0, c) }, this.randomArray = function (n) { var i, o, t, a = []; for (i = 0; i < n; ++i)a[i] = i; for (i = 0; i < n; ++i)t = a[o = parseInt(Math.random() * n, 10)], a[o] = a[i], a[i] = t; return a }, this.loop = function () { var n, i; a && 0 !== c.length && (n = c[r.rand(0, c.length - 1)], (i = e.eq(n))[0].blinking || r.blink(i), l = setTimeout(function () { r.loop() }, r.rand(t.loopMin, t.loopMax))) }, this.blinkOn = function () { a || (a = !0, l = setTimeout(function () { r.loop() }, r.rand(t.loopMin, t.loopMax))) }, this.blinkOff = function () { a && (a = !1, clearTimeout(l)) }, this.bindEvents = function () { s.on("blinkOn", function (n) { r.blinkOn() }), s.on("blinkOff", function (n) { r.blinkOff() }) }, r.repeat()) return !0; t = o, a = !1, l = 0, r.buildHTML(), e = s.find(t.element + ".novacancy"), c = r.newArray(), r.bindEvents(), r.setAppearance(), t.autoOn && r.blinkOn() }(this, function (i) { var o = n.extend({ reblinkProbability: 1 / 3, blinkMin: .01, blinkMax: .5, loopMin: .5, loopMax: 2, color: "ORANGE", glow: ["0 0 80px Orange", "0 0 30px Red", "0 0 6px Yellow"], off: 0, blink: 0, classOn: "on", classOff: "off", element: "data", autoOn: !0 }, i); return o.reblinkProbability *= 100, o.blinkMin *= 1e3, o.blinkMax *= 1e3, o.loopMin *= 1e3, o.loopMax *= 1e3, o }(i)) }) } }(jQuery);



    var swiper1 = new Swiper('.swiper1', {
        slidesPerView: 3,
            spaceBetween: 30,
            pagination: {
        el: '.swiper-pagination1',
                clickable: true,
            },
        });
        var swiper2 = new Swiper('.swiper2', {
        slidesPerView: 3,
            spaceBetween: 30,
            pagination: {
        el: '.swiper-pagination2',
                clickable: true,
            },
        });
        var swiper3 = new Swiper('.swiper3', {
        slidesPerView: 3,
            spaceBetween: 30,
            pagination: {
        el: '.swiper-pagination3',
                clickable: true,
            },
        });

