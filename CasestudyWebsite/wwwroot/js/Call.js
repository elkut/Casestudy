$(() => { // Call.js
    $("#srch").keyup(() => {
        let alldata = JSON.parse(sessionStorage.getItem("allcalls"));
        let filtereddata = alldata.filter((call) => call.employeeName.match(new RegExp($("#srch").val(), 'i')));
        buildCallList(filtereddata, false);
    }); // srch keyup

    document.addEventListener("keyup", e => {
        $("#modalstatus").removeClass(); //remove any existing css on div
        if ($("#CallModalForm").valid()) {
            $("#actionbutton").prop("disabled", false);
            $("#modalstatus").attr("class", "badge bg-success"); //green
            $("#modalstatus").text("enter/update data");
        }
        else {
            $("#modalstatus").attr("class", "badge bg-danger"); //red
            $("#modalstatus").text("fix errors");
            $("#actionbutton").prop("disabled", true);
        }
    });

    $("#CallModalForm").validate({
        rules: {
            TextBoxNotes: { maxlength: 250, required: true},
            ddlEmployees: { required: true },
            ddlProblems:  { required: true },
            ddlTechnicians: { required: true }
        },
        errorElement: "div",
      
        messages: {
            ddlEmployees: { required: "select Employee" },
            ddlProblems: { required: "select Problem" },
            ddlTechnicians: { required: "selecct Tech" },
            textBoxNotes: {required: "required 1-250 cahrs.", maxlength: "required 1-250 chars."}
        }
    }); //CallModalForm.validate

    const formatDate = (date) => {
        let d;
        (date === undefined) ? d = new Date() : d = new Date(Date.parse(date));
        let _day = d.getDate();
        if (_day < 10) { _day = "0" + _day; }
        let _month = d.getMonth() + 1;
        if (_month < 10) { _month = "0" + _month; }
        let _year = d.getFullYear();
        let _hour = d.getHours();
        if (_hour < 10) { _hour = "0" + _hour; }
        let _min = d.getMinutes();
        if (_min < 10) { _min = "0" + _min; }
        return _year + "-" + _month + "-" + _day + "T" + _hour + ":" + _min;
    }; // formatDate


    $("#checkBoxClose").click(() => {
        if ($("#checkBoxClose").is(":checked")) {
            $("#labelDateClosed").text(formatDate().replace("T", " "));
            sessionStorage.setItem("dateClosed", formatDate());
        } else {
            $("#labelDateClosed").text("");
            sessionStorage.setItem("dateClosed", "");
        }
    }); // checkBoxClose

    const setupForUpdate = (id, data) => {
 
        $("#actionbutton").show();
        $("#modaltitle").html("<h4>update call</h4>");
        $('#deletealert').hide();
        $("#DateClosed").show();
      
        $("#checkBoxClose").show();
        $("#closediv").show();
       clearModalFields();
        let callfound = false;

        data.forEach(call => {
            if (call.id === parseInt(id)) {
                callfound = true;
                loadProblemDDL(call.problemId);
                loadEmployeeDDL(call.employeeId);
               loadTechnicainDDL(call.techId);
               sessionStorage.setItem("dateOpened", formatDate(call.dateOpened));
                $("#labelDateOpened").text(formatDate(call.dateOpened).replace("T", " "));
                $("#checkBoxClose").attr("checked", false);
                $("#TextBoxNote").val(call.notes);
                sessionStorage.setItem("id", call.id);
                sessionStorage.setItem("problemId", call.problemId);
                sessionStorage.setItem("employeeId", call.employeeId);
                sessionStorage.setItem("techId", call.techId);
                sessionStorage.setItem("timer", call.timer);
                $("#actionbutton").attr("value","update");
                $("#deleteButton").show();
                $("#CloseRow1").show();
                $("#CloseRow2").show();

                if (!call.openStatus) {
                    callfound = false;
                    $("#labelDateClosed").text(formatDate(call.dateClosed).replace("T", " "));
                    sessionStorage.setItem("dateClosed", formatDate(call.dateClosed));  
                   // $("#labelDateClosed").show();
                    $("#TextBoxNote").attr("readonly", true);
                    $("#ddlProblems").attr("disabled", true);
                    $("#ddlTechnicians").attr("disabled", true);
                    $("#ddlEmployees").attr("disabled", true);
                    $("#checkBoxClose").attr("checked", true);
                    sessionStorage.setItem("dateOpened", formatDate(call.dateOpened));
                    $("#labelDateOpened").text(formatDate(call.dateOpened).replace("T", " "));
                    $("#actionbutton").hide();
                    $("#deleteButton").show();
                }
                $("#modalstatus").text("update data");
                $("#myModal").modal("toggle");
                $("#myModalLabel").text("Update");
            } // if
        }); // data.forEach
    }; // setupForUpdate

    const clearModalFields = () => {
        $("#DateOpened").text("");
        $("#labelDateClosed").text("");
        $("#CloseCall").val("");
        $("#TextBoxNote").val("");
        sessionStorage.removeItem("id");
        sessionStorage.removeItem("problemId");
        sessionStorage.removeItem("employeeId");
        sessionStorage.removeItem("techId");
        sessionStorage.removeItem("dateOpened");
        sessionStorage.removeItem("dateClosed");
        sessionStorage.removeItem("timer");
        loadProblemDDL(-1);
        loadEmployeeDDL(-1);
        loadTechnicainDDL(-1);
        $("#myModal").modal("toggle");
        let validator = $("#CallModalForm").validate();
        validator.resetForm();
    }; // clearModalFields


    const setupForAdd = () => {
        $("#actionbutton").val("add");
        $("#actionbuttonD").val("");
        $("#modaltitle").html("<h4>add call</h4>");
        $("#theModal").modal("toggle");
        $("#modalstatus").text("add new call");
        $("#myModalLabel").text("Add");
        $('#deletealert').hide();
        $('#deleteprompt').hide();
     
        $("#labelDateOpened").text(formatDate().replace("T", " "));
        sessionStorage.setItem("dateOpened", formatDate());
        $("#DateClosed").hide(); 
        $("#CloseCall").hide();
        clearModalFields();
    };// setupForAdd

  

    const buildCallList = (data, usealldata = true) => {
        $("#callList").empty();
        div = $(`<div class="list-group-item text-white bg-secondary row d-flex" style="background-color:cadetblue;" id="status">
                Call Information</div>
                <div class= "list-group-item row d-flex text-center" id="heading" style="background-color:lightblue;">
                <div class="col-4 h4 ">Date</div>
                <div class="col-4 h4 ">For</div>
                <div class="col-4 h4 ">Problem</div>
                </div>`);
        div.appendTo($("#callList"));
        usealldata ? sessionStorage.setItem("allcalls", JSON.stringify(data)) : null;
        btn = $(`<button class="list-group-item row d-flex" style="background-color:lightblue;" id="0">...click to add call</button>`);
        btn.appendTo($("#callList"));
        data.forEach(call => {
            btn = $(`<button class="list-group-item row d-flex" style="background-color:lightblue;" id="${call.id}">`);
            btn.html(`<div class="col-4" style="background-color:lightblue;" id="callDateOpened${call.id}">${formatDate(call.dateOpened).replace("T", " ")}</div>
                    <div class="col-4" style="background-color:lightblue;" id="callEmployeeName$${call.id}">${call.employeeName}</div>
                    <div class="col-4" style="background-color:lightblue;" id="callProblemDescription${call.id}">${call.problemDescription}</div>`
            );
            btn.appendTo($("#callList"));
        }); // forEach
    }; // buildCallList

    const getAll = async (msg) => {
        try {
            $("#callList").text("Finding Call Information...");
            let response = await fetch(`api/call`);
            if (response.ok) {
                let payload = await response.json(); // this returns a promise, so we await it
                buildCallList(payload);
                msg === "" ? // are we appending to an existing message
                    $("#status").text("Calls Loaded") : $("#status").text(`${msg} - Calls Loaded`);
               
            } else if (response.status !== 404) { // probably some other client side error
                let problemJson = await response.json();
                errorRtn(problemJson, response.status);
            } else { // else 404 not found
                $("#status").text("no such path on server");
            } // else

            response = await fetch(`api/problem`);
            if (response.ok) {
                let probs = await response.json(); // this returns a promise, so we await it
                sessionStorage.setItem("allproblems", JSON.stringify(probs));
            } else if (response.status !== 404) { // probably some other client side error
                let problemJson = await response.json();
                errorRtn(problemJson, response.status);
            } else { // else 404 not found
                $("#status").text("no such path on server");
            } // else

            response = await fetch(`api/employee`);
            if (response.ok) {
                let emps = await response.json(); // this returns a promise, so we await it
                sessionStorage.setItem("allemployees", JSON.stringify(emps));
                if (emps) {
                    let techs = emps.filter(employee => employee.isTech === true);
                    (techs.length > 0)
                        ? sessionStorage.setItem("alltechs", JSON.stringify(techs))
                        : sessionStorage.setItem("alltechs", null);
                }
            } else if (response.status !== 404) { // probably some other client side error
                let problemJson = await response.json();
                errorRtn(problemJson, response.status);
            } else { // else 404 not found
                $("#status").text("no such path on server");
            } // else
        }
        catch (error) {
            $("#status").text(error.message);
        }
    }; // getAllCall


    $("#callList").click((e) => {
        if (!e) e = window.event;
        let id = e.target.parentNode.id;
        if (id === "callList" || id === "") {
            id = e.target.id;
        } // clicked on row somewhere else
        if (id !== "status" && id !== "heading") {
            let data = JSON.parse(sessionStorage.getItem("allcalls"));
            clearModalFields();
            id === "0" ? setupForAdd() : setupForUpdate(id, data);
          
        } else {
            return false; // ignore if they clicked on heading or status
        }
    }); // callList click
    

    $("#actionbutton").click(() => {
        $("#actionbutton").val() === "update" ? update() : add();
    });

    const add = async () => {
        try {
            // set up a new client side instance of Call
            call = new Object();
            // pouplate the properties
            call.problemId = parseInt($("#ddlProblems").val());
            call.employeeId = parseInt($("#ddlEmployees").val());
            call.techId = parseInt($("#ddlTechnicians").val());
            call.notes = $("#TextBoxNote").val();
            call.dateOpened = formatDate();
            call.id = 0;
          


            // send the updated back to the server asynchronously using PUT
            let response = await fetch("api/call", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json; charset=utf-8"
                },
                body: JSON.stringify(call)
            });
            if (response.ok) // or check for response.status
            {
                let data = await response.json();
                getAll(data.msg);
            } else if (response.status !== 404) { // probably some other client side error
                let problemJson = await response.json();
                errorRtn(problemJson, response.status);
            } else { // else 404 not found
                $("#status").text("no such path on server");
            } // else
        } catch (error) {
            $("#status").text(error.message);
        } // try/catch
        $("#myModal").modal("toggle");
    }; // add

    const update = async () => {
        try {
            // set up a new client side instance of Call
            call = new Object();
            // pouplate the properties
            call.problemId = parseInt($("#ddlProblems").val());
            call.employeeId = parseInt($("#ddlEmployees").val());
            call.techId = parseInt($("#ddlTechnicians").val());
            call.dateOpened = sessionStorage.getItem("dateOpened");
            call.notes = $("#TextBoxNote").val();
            call.id = parseInt(sessionStorage.getItem("id"));
            call.timer = sessionStorage.getItem("timer");
            if ($("#checkBoxClose").is(":checked")) {
                call.openStatus = false;
                call.dateClosed = sessionStorage.getItem("dateClosed");
            }
            else {
                call.openStatus = true;
            }
            // send the updated back to the server asynchronously using PUT
            let response = await fetch("api/call", {
                method: "PUT",
                headers: { "Content-Type": "application/json; charset=utf-8" },
                body: JSON.stringify(call)
            });
            if (response.ok) // or check for response.status
            {
                let data = await response.json();
                getAll(data.msg);
            } else if (response.status !== 404) { // probably some other client side error
                let problemJson = await response.json();
                errorRtn(problemJson, response.status);
            } else { // else 404 not found
                $("#status").text("no such path on server");
            } // else
        } catch (error) {
            $("#status").text(error.message);
        } // try/catch
        $("#myModal").modal("toggle");
    }; // update

    const loadProblemDDL = (callprob) => {
        html = '';
        $('#ddlProblems').empty();
        let allproblems = JSON.parse(sessionStorage.getItem('allproblems'));
        allproblems.forEach((prob) => { html += `<option value="${prob.id}">${prob.description}</option>` });
        $('#ddlProblems').append(html);
        $('#ddlProblems').val(callprob);
    }; // loadProblemDDL

    const loadEmployeeDDL = (callemp) => {
        html = '';
        $('#ddlEmployees').empty();
        let allEmployees = JSON.parse(sessionStorage.getItem('allemployees'));
        allEmployees.forEach((emp) => { html += `<option value="${emp.id}">${emp.firstname}</option>` });
        $('#ddlEmployees').append(html);
        $('#ddlEmployees').val(callemp);
    }; // loadEmployeeDDL

    const loadTechnicainDDL = (calltech) => {
        html = '';
        $('#ddlTechnicians').empty();
        let allTechnicians = JSON.parse(sessionStorage.getItem('alltechs'));
        allTechnicians.forEach((tech) => { html += `<option value="${tech.id}">${tech.firstname}</option>` });
        $('#ddlTechnicians').append(html);
        $('#ddlTechnicians').val(calltech);
    }; // loadTechnicainDDL

    const _delete = async () => {
        try {
            let response = await fetch(`api/call/${sessionStorage.getItem('id')}`, {
                method: 'DELETE',
                headers: { 'Content-Type': 'application/json; charset=utf-8' }
            });
            if (response.ok) // or check for response.status
            {
                let data = await response.json();
                getAll(data.msg);
            } else {
                $('#status').text(`Status - ${response.status}, Problem on delete server side, see server console`);
            } // else
            $('#myModal').modal('toggle');
        } catch (error) {
            $('#status').text(error.message);
        }
    }; // delete

    $('#deleteprompt').click((e) => {
        $('#deletealert').show();
    });
    $('#deletenobutton').click((e) => {
        $('#deletealert').hide();
    });
    $('#deletebutton').click(() => {
        _delete();
    });

    getAll("");                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                  (""); // first grab the data from the server
}); // jQuery ready method

// server was reached but server had a problem with the call
const errorRtn = (problemJson, status) => {
    if (status > 499) {
        $("#status").text("Problem server side, see debug console");
    } else {
        let keys = Object.keys(problemJson.errors)
        problem = {
            status: status,
            statusText: problemJson.errors[keys[0]][0], // first error
        };
        $("#status").text("Problem client side, see browser console");
        console.log(problem);
    } // else
}
