///Created by : Sub Maj M Sanal Kumar on 04 Aug 23
// Reviewed by : Mr Rahul on 04 Aug 23
// GET: Projects/Create

function populateddlStakeHolder(paramValue) {
  
    $.ajax({
        url: '/Ddl/ddlStackHlder',
        data: { paramValue: paramValue },
        method: 'GET',
        success: function (data) {
            const odc = document.getElementById('ddlStakeHolderId');
            var option = document.createElement("option");
            option.value = "-1";
            option.text = "---- Select ----";
            odc.appendChild(option);
            //console.log(data);
            for (var i = 0; i < data.length; i++) {
                var item = data[i];
                var opt1 = document.createElement("option");
                opt1.value = item.stakeHolderId;
                opt1.text = item.stakeHolder;
                odc.appendChild(opt1);
            };
        }
    });

}

//Proj Details and Edit
function populateddlActions(paramValue) {

    $.ajax({
        url: '/Ddl/ddlActions',
        data: { paramValue: paramValue },
        method: 'GET',
        success: function (data) {
            const odc = document.getElementById('ddlActions');
            var option = document.createElement("option");
            option.value = "-1";
            option.text = "---- Select ----";
            odc.appendChild(option);
            //console.log(data);
            for (var i = 0; i < data.length; i++) {
                var item = data[i];
                var opt1 = document.createElement("option");
                opt1.value = item.actionsId;
                opt1.text = item.actions;
                odc.appendChild(opt1);
            };

        }

    });

}


function ddlActionsNew(paramValue) {

    $.ajax({
        url: '/Ddl/ddlActions',
        data: { paramValue: paramValue },
        method: 'GET',
        success: function (data) {
            const odc = document.getElementById('ddlActionsID');
            var option = document.createElement("option");
            option.value = "-1";
            option.text = "---- Select ----";
            odc.appendChild(option);
           
            for (var i = 0; i < data.length; i++) {
                var item = data[i];
                var opt1 = document.createElement("option");
                opt1.value = item.actionsId;
                opt1.text = item.actions;
                odc.appendChild(opt1);
            };



        }



    });

}
function populateddlUnitedit(paramValue) {

    $.ajax({
        url: '/Ddl/ddlUnit',
        data: { paramValue: paramValue },
        method: 'GET',
        success: function (data) {
            const odc = document.getElementById('ddlUnitId');
            var option = document.createElement("option");
            option.value = "-1";
            option.text = "---- Select ----";
            odc.appendChild(option);
            //console.log(data);
            for (var i = 0; i < data.length; i++) {
                var item = data[i];
                var opt1 = document.createElement("option");
                opt1.value = item.unitid;
                opt1.text = item.unitName;
                odc.appendChild(opt1);
            };



        }

    });

}


function populateddlUnit(paramValue) {

    $.ajax({
        url: '/Ddl/ddlUnit',
        data: { paramValue: paramValue },
        method: 'GET',
        success: function (data) {
            const odc = document.getElementById('ddlUnitId');
            var option = document.createElement("option");
            option.value = "-1";
            option.text = "---- Select ----";
            odc.appendChild(option);
            //console.log(data);
            for (var i = 0; i < data.length; i++) {
                var item = data[i];
                var opt1 = document.createElement("option");
                opt1.value = item.unitid;
                opt1.text = item.unitName;
                odc.appendChild(opt1);
            };



        }

    });

}





function populatfwdUnits(paramValue) {
   
    $.ajax({
        url: '/Ddl/ddlFwdUnits',
        data: { paramValue: paramValue },
        method: 'GET',
        success: function (data) {
            const odc = document.getElementById('ddlUnitId');
            var option = document.createElement("option");
            option.value = "-1";
            option.text = "---- Select ----";
            odc.appendChild(option);
          
            for (var i = 0; i < data.length; i++) {
                var item = data[i];
                var opt1 = document.createElement("option");
                opt1.value = item.unitid;
                opt1.text = item.unitName;
                odc.appendChild(opt1);
            };



        }

    });

}


function XpopulatfwdUnits(paramValue) {

    $.ajax({
        url: '/Ddl/ddlFwdUnits',
        data: { paramValue: paramValue },
        method: 'GET',
        success: function (data) {
           
            const odc = document.getElementById('XddlUnitIds');
            var option = document.createElement("option");
            option.value = "-1";
            option.text = "---- Select ----";
            odc.appendChild(option);

            for (var i = 0; i < data.length; i++) {
                var item = data[i];
                var opt1 = document.createElement("option");
                opt1.value = item.unitid;
                opt1.text = item.unitName;
                odc.appendChild(opt1);
            };

        }

    });

}




function populateddlAppType(paramValue) {

    $.ajax({
        url: '/Ddl/DdlAppType',
        data: { paramValue: paramValue },
        method: 'GET',
        success: function (data) {
            const odc = document.getElementById('ddlAppType');
            var option = document.createElement("option");
            option.value = "-1";
            option.text = "---- Select ----";
            odc.appendChild(option);
            //console.log(data);
            for (var i = 0; i < data.length; i++) {
                var item = data[i];
               
                const opt1 = document.createElement('option')
                opt1.value = item.apptype;
                opt1.text = item.appDesc;
                odc.appendChild(opt1);
            };



        }

    });

}


function populateddlAppTypeEdit(paramValue) {

    $.ajax({
        url: '/Ddl/DdlAppType',
        data: { paramValue: paramValue },
        method: 'GET',
        success: function (data) {
            const odc = document.getElementById('ddlAppTypeEdit');
            var optionK = document.createElement("option");
            optionK.value = "-1";
            optionK.text = "---- Select ----";
            odc.appendChild(optionK);
           
            for (var i = 0; i < data.length; i++) {
                var item = data[i];
                
                var opt1 = document.createElement("option");
                opt1.value = item.apptype;
                opt1.text = item.appDesc;
                odc.appendChild(opt1);
            };



        }

    });

}


function populateddlfwdUnit(paramValue) {

    $.ajax({
        url: '/Ddl/ddlFwdUnit',
        data: { paramValue: paramValue },
        method: 'GET',
        success: function (data) {
            const odc = document.getElementById('ddlUnitId');
            var optionG = document.createElement("option");
            optionG.value = "-1";
            optionG.text = "---- Select ----";
            odc.appendChild(optionG);
           
            for (var i = 0; i < data.length; i++) {
                var item = data[i];
                var itemValues = Object.values(item);
                var option = document.createElement("option");
                option.value = item.unitid;
                option.text = item.unitName;
                odc.appendChild(option);
            };



        }

    });

}




function popRestddlfwdUnit(projIds) {

    $.ajax({
        url: '/Ddl/ddlResFwdUnit',
        data: { ProjIds: projIds },
        method: 'GET',
        success: function (data) {
            const odc = document.getElementById('ddlUnitId');
            var optionG = document.createElement("option");
            optionG.value = "";
            optionG.text = "---- Select ----";
            odc.appendChild(optionG);

            for (var i = 0; i < data.length; i++) {
                var item = data[i];
                var itemValues = Object.values(item);
                var option = document.createElement("option");
                option.value = item.unitid;
                option.text = item.unitName;
                odc.appendChild(option);
            };



        }

    });

}


function populateddlStatus(paramValue) {

    $.ajax({
        url: '/Ddl/ddlStatus',
        data: { paramValue: paramValue },
        method: 'GET',
        success: function (data) {
            const odc = document.getElementById('ddlStatusId');
            var optionF = document.createElement("option");
            optionF.value = "-1";
            optionF.text = "---- Select ----";
            odc.appendChild(optionF);
           
            for (var i = 0; i < data.length; i++) {
                var item = data[i];
               
                var option = document.createElement("option");
                option.value = item.StatusId;
                option.text = item.Status;
                odc.appendChild(option);
            };



        }

    });

}

function populateStages(paramValue, laststagesId, projIds) {
        
        $.ajax({
            url: '/Ddl/GetAllStages',
            data: {
                paramValue: paramValue,
                projIds: projIds
            },
            method: 'GET',
            success: function (data) {

                const odc = document.getElementById('ddlStages');
                var optionS = document.createElement("option");
                optionS.value = "";
                optionS.text = "---- Select ----";
                odc.appendChild(optionS);

                for (var i = 0; i < data.length; i++) {
                    var item = data[i];

                    var option = document.createElement("option");
                    option.value = item.stagesId;
                    option.text = item.stages;
                    odc.appendChild(option);



                };



            }

        });

}



function getStatusByStage(stageIds, currentstakeholderid)
{
   
    $.ajax({

        url: '/Ddl/GetStatusByStage',
        data: { stageIds: stageIds },
        method: 'GET',
        success: function (data) {

            const odc = document.getElementById('ddlStatus');
            var optionX = document.createElement("option");
            optionX.value = "";
            optionX.text = "---- Select ----";
            odc.appendChild(optionX);

            for (var i = 0; i < data.length; i++) {
                var item = data[i];
                var itemValues = Object.values(item);
                var option = document.createElement("option");
                option.value = item.statusId;
                option.text = item.status;
                odc.appendChild(option);
                if ((stageIds == 2 && (option.value == 5 || option.value == 7 || option.value == 11)) ||
                    (stageIds == 1 && option.value == 1 || option.value == 2 || option.value == 4 || option.value==31)) {
                    odc.removeChild(option);
                }

            };

        }

    });

}

function getActionsByStatus(selectedStatusId) {
     
    $.ajax({

        url: '/Ddl/GetActionsByStatus',
        data: { selectedStatusId: selectedStatusId },
        method: 'GET',
        success: function (data) {

            const odc = document.getElementById('ddlActionsID');
            var optionA = document.createElement("option");
            optionA.value = "-1";
            optionA.text = "---- Select ----";
            odc.appendChild(optionA);

            for (var i = 0; i < data.length; i++) {
                var item = data[i];
               
                var option = document.createElement("option");
                option.value = item.actionsId;
                option.text = item.actions;
                odc.appendChild(option);
            };



        }

    });

}
// fwd process
function EditActionsByStatus(selectedStatusId, selectedStageId, projIds) {

    
    $.ajax({

        url: '/Ddl/GetActiByStageStat',
        data: {
            selectedStatusId: selectedStatusId,
            selectedStageId: selectedStageId,
            projIds: projIds

        },
        method: 'GET',
        success: function (data) {

            const odc = document.getElementById('ddlActions');
            var optionA = document.createElement("option");
            optionA.value = "";
            optionA.text = "---- Select ----";
            odc.appendChild(optionA);

            for (var i = 0; i < data.length; i++) {
                var item = data[i];

                var option = document.createElement("option");
                option.value = item.actionsId;
                option.text = item.actions;
                odc.appendChild(option);
            };



        }

    });

}


function GActionsByStatusNew(selectedStatusId) {

    $.ajax({

        url: '/Ddl/GetActionsByStatus',
        data: { selectedStatusId: selectedStatusId },
        method: 'GET',
        success: function (data) {

            const odc = document.getElementById('ddlActions');
            var optionB = document.createElement("option");
            optionB.value = "-1";
            optionB.text = "---- Select ----";
            odc.appendChild(optionB);

            for (var i = 0; i < data.length; i++) {
                var item = data[i];
                
                var option = document.createElement("option");
                option.value = item.actionsId;
                option.text = item.actions;
                odc.appendChild(option);
            };



        }

    });

}
