$(() => { // hellopdf.js
    $("#pdfbutton").click(async (e) => {
        try {
            $("#lblstatus").text("generating report on server - please wait...");
            let response = await fetch(`api/helloreport`);
            if (!response.ok) // check for response.status
                throw new Error(`Status - ${response.status}, Text - ${response.statusText}`);
            let data = await response.json(); // this returns a promise, so we await it
            data.msg === "Report Generated"
                ? window.open("/pdfs/hellow.pdf")
                : $("#lblstatus").text("problem generating report");
        } catch (error) {
            $("#lblstatus").text(error.message);
        } // try/catch
    }); // button click
}); // jQuery