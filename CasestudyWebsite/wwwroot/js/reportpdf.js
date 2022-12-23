$(() => { // reportpdf.js
    $("#pdfbutton").click(async (e) => {
        try {
            $("#lblstatus").text("generating report on server - please wait...");
            let response = await fetch(`api/employeereport`);
            if (!response.ok) // check for response.status
                throw new Error(`Status - ${response.status}, Text - ${response.statusText}`);
            let data = await response.json(); // this returns a promise, so we await it
            data.msg === "Report Generated"
                ? window.open("/pdfs/employeelist.pdf")
                : $("#lblstatus").text("problem generating report");
        } catch (error) {
            $("#lblstatus").text(error.message);
        } // try/catch
    }); // button click

    $("#callspdfbutton").click(async (e) => {
        try {
            $("#lblstatus").text("generating report on server - please wait...");
            let response = await fetch(`api/callreport`);
            if (!response.ok) // check for response.status
                throw new Error(`Status - ${response.status}, Text - ${response.statusText}`);
            let data = await response.json(); // this returns a promise, so we await it
            data.msg === "Report Generated"
                ? window.open("/pdfs/calllist.pdf")
                : $("#clblstatus").text("problem generating report");
        } catch (error) {
            $("#clblstatus").text(error.message);
        } // try/catch
    }); // button click
}); // jQuery