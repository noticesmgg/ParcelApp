import DataGrid, { Column, Paging, Pager, FilterRow, SearchPanel, Sorting, Editing, Summary, TotalItem, ColumnChooser, StateStoring, Position, ColumnChooserSearch, ColumnChooserSelection, Scrolling, Toolbar, Item, HeaderFilter, LoadPanel } from "devextreme-react/data-grid";
import CustomStore from "devextreme/data/custom_store";
import "devextreme/dist/css/dx.light.css";
import "./Parcel.css";
import notify from "devextreme/ui/notify";
import type dxDataGrid from "devextreme/ui/data_grid";
import { useRef, useState } from "react";
import type { EditingStartEvent, RowDblClickEvent } from "devextreme/ui/data_grid";
import "./ParcelEditBox.css";
import HeaderProfile from "../Profile/Profile";
import type { GroupItemTemplateData } from "devextreme/ui/form";
import type { DxElement } from "devextreme/core/element";







const baseOptions = {
    displayTime: 2000,
    position: {
        my: "top",
        at: "top",
        of: "#parcel-content",
        offset: "0 60"
    },
    width: 300
};

export type ParcelDO = {
    Id?: number | null;
    ParcelNumber: string;
    ShortParcel: string;
    Street: string;
    City: string;
    State: string;
    ZipCode?: number | null;
    AskingPrice?: number | null;
    UpdatedAskingPrice?: number | null;
    AdDate?: Date | string | null;
    BidOffDate?: Date | string | null;
    LastDateToApply?: Date | string | null;
    Acreage?: number | null;
    SquareFoot?: number | null;
    Dimensions: string;
    HasDemo?: boolean | null;
    PermitStatus: string;
    PropertyStatus: string;
    PropertyClassification: string;
    Source: string;
    Owner: string;
    CreatedDate?: Date | string | null;
    UpdatedDate?: Date | string | null;
    CreatedBy: string;
    UpdatedBy: string;
};

export type ApplicationStatusDO = {
    Id?: number | null;
    LandBankId?: number | null;
    AccountId: string;
    SubmitDate?: Date | string | null;
    ReSubmitDate?: Date | string | null;
    AcceptedDate?: Date | string | null;
    ApplicationNumber: string;
    Status: string;
    OurBid?: number | null;
    Competitor: string;
    WinningBid?: number | null;
    CreatedDate?: Date | string | null;
    UpdatedDate?: Date | string | null;
    CreatedBy: string;
    UpdatedBy: string;
};

export type BookmarkDO = {
    Id?: number | null;
    LandBankId?: number | null;
    Interest: string;
    UpperLimit?: number | null;
    CompLimit: string;
    Notes: string;
    CreatedDate?: Date | string | null;
    UpdatedDate?: Date | string | null;
    CreatedBy: string;
    UpdatedBy: string;
};

type ParcelEditModel = {
    Parcel?: ParcelDO;
    Bookmark?: BookmarkDO;
    ApplicationStatus?: ApplicationStatusDO;
};


const API_URL = import.meta.env.VITE_API_URL;
let parcelCache: ParcelDO[] = [];

const parcelStore = new CustomStore<ParcelDO, number>({
    key: "Id",
    load: async () => {
        if (parcelCache.length > 0) {
            return parcelCache;
        }
        console.log(import.meta.env.VITE_ENV)
        const response = await fetch(`${API_URL}/landbank?format=json`);
        if (!response.ok) throw new Error("Failed to load parcels");
        const data = await response.json();
        parcelCache = data;
        return data;
    },
    byKey: async (key) => {
        const parcel = parcelCache.find(p => p.Id === key);
        if (!parcel) throw new Error("Parcel not found");
        return parcel;
    },
    update: async (key, values) => {
        const index = parcelCache.findIndex(p => p.Id === key);
        if (index === -1) {
            notify("Record not found", "error", 3000);
            throw new Error("Record not found");
        }

        const updated = {
            ...parcelCache[index],
            ...values,
            Id: key
        };

        try {
            const response = await fetch(`${API_URL}/landbank/${key}`, {
                method: "PUT",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(updated)
            });

            const success = await response.json();

            if (!success) {
                throw new Error("Failed to update parcel");
            }

            parcelCache[index] = updated;
            parcelStore.push([{
                type: "update",
                key: key,
                data: updated
            }]);

            notify({ ...baseOptions, message: "Parcel updated successfully", type: "success" });

            return updated;
        } catch (err) {
            notify({ ...baseOptions, message: "Failed to update parcel", type: "error" });
            throw err;
        }
    },
    remove: async (key) => {
        try {
            const response = await fetch(`${API_URL}/landbank/${key}`, {
                method: "DELETE"
            });

            if (!response.ok) throw new Error("Delete failed");

            parcelCache = parcelCache.filter(p => p.Id !== key);

            notify({ ...baseOptions, message: "Parcel deleted", type: "success" });
        } catch (err) {
            notify({ ...baseOptions, message: "Delete failed", type: "error" });
            throw err;
        }
    }
});

export default function ParcelGrid() {

    const gridRef = useRef<dxDataGrid<ParcelDO, number> | undefined>(null);
    const [editingRow, setEditingRow] = useState<ParcelEditModel>();
    const imageInputRef = useRef<HTMLInputElement>(null);
    const videoInputRef = useRef<HTMLInputElement>(null);


    const OnRowDoubleClick = (e: RowDblClickEvent) => {
        if (e.rowIndex === undefined || e.rowType !== "data") return;

        gridRef.current?.editRow(e.rowIndex);
    }

    const OnEditingStart = async (e: EditingStartEvent<ParcelDO, number>) => {
        const parcel = e.data as ParcelDO;

        const editModel: ParcelEditModel = {
            Parcel: parcel
        };

        try {
            const bookmarkRes = await fetch(
                `${API_URL}/lbbookmarks/?format=json&LandBankId=${parcel.Id}`
            );
            if (bookmarkRes.ok) {
                editModel.Bookmark = await bookmarkRes.json();
            }
        } catch {
            notify({ ...baseOptions, message: "Failed to load bookmark", type: "error" });
        }

        try {
            const appRes = await fetch(
                `${API_URL}/lbapplicationstatus/?format=json&LandBankId=${parcel.Id}`
            );
            if (appRes.ok) {
                editModel.ApplicationStatus = await appRes.json();
            }
        } catch {
            notify({ ...baseOptions, message: "Failed to load application status", type: "error" });
        }

        setEditingRow(editModel);
    }

    const Images = [
        {
            id: 1,
            name: "Front View",
            url: "https://picsum.photos/300/200?random=1"
        },
        {
            id: 2,
            name: "Side View",
            url: "https://picsum.photos/300/200?random=2"
        },
        {
            id: 3,
            name: "Aerial View",
            url: "https://picsum.photos/300/200?random=3"
        }
    ]

    const Videos = [
        {
            id: 1,
            name: "Site Walkthrough",
            url: "https://www.w3schools.com/html/mov_bbb.mp4"
        },
        {
            id: 2,
            name: "Drone Footage",
            url: "https://www.w3schools.com/html/movie.mp4"
        }
    ]

    return (
        <div className="parcel-app">
            <header className="parcel-header">
                <div className="parcel-header-left">
                    <h1>Parcel Management</h1>
                    <span className="subtitle">Land Bank & Property Records</span>
                </div>
                <div className="parcel-header-right">
                    <HeaderProfile />
                </div>
            </header>

            <main className="parcel-content">
                <DataGrid
                    onInitialized={(e) => (gridRef.current = e.component)}
                    dataSource={parcelStore}
                    height="calc(100vh - 64px)"
                    width="100%"
                    showBorders={true}
                    columnAutoWidth={true}
                    rowAlternationEnabled={true}
                    hoverStateEnabled={true}
                    repaintChangesOnly={true}
                    allowColumnReordering={true}
                    allowColumnResizing={true}
                    columnResizingMode="widget"
                    showColumnLines={true}
                    showRowLines={true}
                    wordWrapEnabled={false}
                    columnHidingEnabled={false}
                    onRowDblClick={OnRowDoubleClick}
                    onEditingStart={OnEditingStart}
                >
                    <Toolbar>
                        <Item name="searchPanel" location="before" />
                        <Item name="columnChooserButton" location="after" />
                    </Toolbar>
                    <Scrolling columnRenderingMode="standard" mode="standard" showScrollbar="always" useNative={true} />
                    <StateStoring enabled={true} type="localStorage" storageKey="parcel-grid-state" />
                    <ColumnChooser enabled={true} mode="select" height='400px' width='400px'>
                        <Position my="right top" at="right bottom" of=".dx-datagrid-column-chooser-button" />
                        <ColumnChooserSearch enabled={true} editorOptions={{ placeholder: 'Search column' }} />
                        <ColumnChooserSelection allowSelectAll={true} selectByClick={true} recursive={true} />
                    </ColumnChooser>
                    <SearchPanel visible={true} width={260} highlightCaseSensitive={true} highlightSearchText={true} />
                    <FilterRow visible={true} />
                    <HeaderFilter visible={true} search={{ enabled: true }} allowSelectAll={true} searchMode="contains" height={500} width={300} />
                    <Sorting mode="multiple" />
                    <Paging defaultPageSize={50} />
                    <Pager visible={true} showPageSizeSelector={true} allowedPageSizes={[10, 20, 50]} showInfo={true} />
                    <LoadPanel enabled={true} shading={true} showPane={true} />
                    <Editing
                        mode="popup"
                        useIcons={true}
                        confirmDelete={true}
                        allowUpdating={false}
                        allowDeleting={false}
                        allowAdding={false}
                        popup={{
                            title: editingRow?.Parcel?.Street
                                ? `Edit Parcel — ${editingRow.Parcel.Street}`
                                : "Edit Parcel",
                            showTitle: true,
                            width: 720,
                            maxHeight: "80vh",
                            shading: true,
                            shadingColor: "rgba(0,0,0,0.45)",
                            hideOnOutsideClick: false,
                            dragEnabled: false,
                            resizeEnabled: false,
                            position: { my: "center", at: "center", of: window }
                        }}

                        form={{
                            formData: editingRow,
                            colCount: 2,
                            labelLocation: "top",
                            items: [{
                                itemType: "tabbed",
                                cssClass: "parcel-tabs",
                                tabPanelOptions: {
                                    height: "63vh",
                                    width: "670",
                                    scrollByContent: true,
                                    scrollingEnabled: true,
                                    deferRendering: false,
                                    animationEnabled: true,
                                    showNavButtons: true,
                                    swipeEnabled: true,
                                },
                                tabs: [
                                    {
                                        title: "Parcel Info",
                                        colCount: 2,
                                        items: [
                                            { dataField: "Parcel.Street" },
                                            { dataField: "Parcel.City" },
                                            { dataField: "Parcel.State" },
                                            { dataField: "Parcel.ZipCode" },
                                            { dataField: "Parcel.AskingPrice", editorOptions: { format: { type: "currency", precision: 0 }, stylingMode: "outlined" } },
                                            { dataField: "Parcel.UpdatedAskingPrice", editorOptions: { format: { type: "currency", precision: 0 }, stylingMode: "outlined" } },
                                            { dataField: "Parcel.Acreage" },
                                            { dataField: "Parcel.SquareFoot" },
                                            { dataField: "Parcel.Dimensions" },
                                            { dataField: "Parcel.Source" },
                                            { dataField: "Parcel.Owner" }
                                        ]
                                    },
                                    {
                                        title: "Dates & Status",
                                        colCount: 2,
                                        items: [
                                            { dataField: "Parcel.AdDate", editorType: "dxDateBox" },
                                            { dataField: "Parcel.BidOffDate", editorType: "dxDateBox" },
                                            { dataField: "Parcel.LastDateToApply", editorType: "dxDateBox" },
                                            { dataField: "Parcel.HasDemo" },
                                            { dataField: "Parcel.PermitStatus" },
                                            { dataField: "Parcel.PropertyStatus" },
                                            { dataField: "Parcel.PropertyClassification" },
                                        ]
                                    },
                                    {
                                        title: "Bookmark Status",
                                        colCount: 2,
                                        items: [
                                            { dataField: "Bookmark.Id", editorType: "dxNumberBox", editorOptions: { readOnly: true } },
                                            { dataField: "Bookmark.LandBankId", editorType: "dxNumberBox", editorOptions: { readOnly: true } },
                                            { dataField: "Bookmark.Interest", editorType: "dxSelectBox", editorOptions: { items: ["Yes", "No", "Conditional", "Pending", "Ignore"] } },
                                            { dataField: "Bookmark.UpperLimit", editorType: "dxNumberBox" },
                                            { dataField: "Bookmark.CompLimit", editorType: "dxTextBox" },
                                            { dataField: "Bookmark.Notes", editorType: "dxTextBox" },
                                        ]
                                    },
                                    {
                                        title: "Application Status",
                                        colCount: 2,
                                        items: [
                                            { dataField: "ApplicationStatus.Id", editorType: "dxNumberBox", editorOptions: { readOnly: true } },
                                            { dataField: "ApplicationStatus.LandBankId", editorType: "dxNumberBox", editorOptions: { readOnly: true } },
                                            { dataField: "ApplicationStatus.AccountId", editorType: "dxTextBox", },
                                            { dataField: "ApplicationStatus.SubmitDate", editorType: "dxDateBox", },
                                            { dataField: "ApplicationStatus.ReSubmitDate", editorType: "dxDateBox", },
                                            { dataField: "ApplicationStatus.AcceptedDate", editorType: "dxDateBox", },
                                            { dataField: "ApplicationStatus.ApplicationNumber", editorType: "dxTextBox", },
                                            { dataField: "Status", editorType: "dxSelectBox", editorOptions: { items: ["Bid Submitted", "Bid Accepted", "Pending", "Reverted", "Re-Submitted", "Accepted", "Bid Lost", "Bid Won"], stylingMode: "outlined" } },
                                            { dataField: "OurBid", editorType: "dxNumberBox", editorOptions: { format: "currency" } },
                                            { dataField: "Competitor", editorType: "dxTextBox", },
                                            { dataField: "WinningBid", editorType: "dxNumberBox", editorOptions: { format: "currency" } },
                                        ]
                                    },
                                    {
                                        title: "Audit Info",
                                        colCount: 2,
                                        items: [
                                            { dataField: "Id", editorOptions: { readOnly: true } },
                                            { dataField: "ParcelNumber", editorOptions: { readOnly: true } },
                                            { dataField: "ShortParcel", editorOptions: { readOnly: true } },
                                        ]
                                    },
                                    {
                                        title: "Upload Images and Videos",
                                        colCount: 2,
                                        items: [
                                            {
                                                itemType: "button",
                                                horizontalAlignment: "left",
                                                buttonOptions: {
                                                    text: "Upload Images",
                                                    icon: "image",
                                                    type: "default",
                                                    onClick: () => imageInputRef.current?.click()
                                                }
                                            },
                                            {
                                                itemType: "button",
                                                horizontalAlignment: "left",
                                                buttonOptions: {
                                                    text: "Upload Videos",
                                                    icon: "video",
                                                    type: "default",
                                                    onClick: () => videoInputRef.current?.click()
                                                }
                                            },
                                            {
                                                colSpan: 2,
                                                label: { text: "Attached Images" },
                                                template: (_data: GroupItemTemplateData, itemElement: DxElement) => {
                                                    itemElement.innerHTML = "";

                                                    const container = document.createElement("div");
                                                    container.className = "image-gallery-container";

                                                    const images = Images;

                                                    if (images.length === 0) {
                                                        container.innerHTML = "<p>No images uploaded yet.</p>";
                                                    } else {
                                                        images.forEach(img => {
                                                            const imgTag = document.createElement("img");
                                                            imgTag.src = img.url;
                                                            imgTag.alt = img.name || "Parcel Image";
                                                            imgTag.style.cssText = `
                                                                                    width: 100px;
                                                                                    height: 100px;
                                                                                    margin: 6px;
                                                                                    object-fit: cover;
                                                                                    border-radius: 6px;
                                                                                    border: 1px solid #ddd;
                                                                                    cursor: pointer;
                                                                                `;
                                                            imgTag.onclick = () => window.open(img.url, "_blank");
                                                            container.appendChild(imgTag);
                                                        });
                                                    }
                                                    itemElement.appendChild(container);
                                                }
                                            },
                                            {
                                                colSpan: 2,
                                                label: { text: "Attached Videos" },
                                                template: (_data: GroupItemTemplateData, itemElement: DxElement) => {
                                                    const container = document.createElement("div");
                                                    container.className = "video-gallery-container";

                                                    const videos = Videos;

                                                    if (videos.length === 0) {
                                                        container.innerHTML = "<p>No videos uploaded yet.</p>";
                                                    } else {
                                                        videos.forEach(vid => {
                                                            const videoWrapper = document.createElement("div");
                                                            videoWrapper.style.display = "inline-block";
                                                            videoWrapper.style.margin = "8px";

                                                            const video = document.createElement("video");
                                                            video.src = vid.url;
                                                            video.controls = true;
                                                            video.width = 180;
                                                            video.height = 120;
                                                            video.style.borderRadius = "6px";
                                                            video.style.border = "1px solid #ddd";
                                                            video.style.objectFit = "cover";

                                                            const label = document.createElement("div");
                                                            label.innerText = vid.name || "Video";
                                                            label.style.fontSize = "12px";
                                                            label.style.marginTop = "4px";
                                                            label.style.textAlign = "center";
                                                            label.style.color = "#555";

                                                            videoWrapper.appendChild(video);
                                                            videoWrapper.appendChild(label);
                                                            container.appendChild(videoWrapper);
                                                        });
                                                    }

                                                    itemElement.appendChild(container);
                                                }
                                            }

                                        ]
                                    }
                                ]
                            }]
                        }}
                    />


                    <Column dataField="Id" visible={false} allowEditing={false} dataType="number" caption="ID" />
                    <Column dataField="ParcelNumber" caption="Parcel #" allowEditing={false} fixed fixedPosition="left" width={150} />
                    <Column dataField="ShortParcel" caption="Short Parcel" fixed fixedPosition="left" width={150} />
                    <Column dataField="Street" width={200} />
                    <Column dataField="City" width={100} />
                    <Column dataField="State" width={80} />
                    <Column dataField="ZipCode" caption="ZIP" width={100} />
                    <Column dataField="AskingPrice" caption="Asking Price" dataType="number" format="currency" width={150} />
                    <Column dataField="UpdatedAskingPrice" caption="Updated Asking Price" dataType="number" format="currency" width={200} />
                    <Column dataField="AdDate" caption="Ad Date" dataType="date" width={150} />
                    <Column dataField="BidOffDate" caption="Bid Off Date" dataType="date" width={150} />
                    <Column dataField="LastDateToApply" caption="Last Apply Date" dataType="date" width={150} />
                    <Column dataField="Acreage" caption="Acreage" dataType="number" width={100} />
                    <Column dataField="SquareFoot" caption="Sq Ft" dataType="number" width={100} />
                    <Column dataField="Dimensions" width={240} />
                    <Column dataField="HasDemo" caption="Has Demo" dataType="boolean" width={120} />
                    <Column dataField="PermitStatus" caption="Permit Status" width={120} />
                    <Column dataField="PropertyStatus" caption="Status" width={100} />
                    <Column dataField="PropertyClassification" caption="Classification" width={150} />
                    <Column dataField="Source" width={150} />
                    <Column dataField="Owner" width={150} />
                    <Column dataField="CreatedDate" caption="Created" dataType="datetime" visible={false} allowEditing={false} />
                    <Column dataField="UpdatedDate" caption="Updated" dataType="datetime" visible={false} allowEditing={false} />
                    <Column dataField="CreatedBy" visible={false} allowEditing={false} />
                    <Column dataField="UpdatedBy" visible={false} allowEditing={false} />
                    <Summary >
                        <TotalItem
                            column="Acreage"
                            summaryType="sum"
                            valueFormat={{ type: "fixedPoint", precision: 3 }}
                            displayFormat="Total Acreage: {0}"
                        />
                        <TotalItem
                            column="AskingPrice"
                            summaryType="avg"
                            valueFormat="currency"
                            displayFormat="Avg Asking Price: {0}"
                        />
                    </Summary>
                </DataGrid>
            </main>
            {/* <footer className="parcel-footer">
                © {new Date().getFullYear()} Parcel Management System · Internal Use Only
            </footer> */}

            <input
                ref={imageInputRef}
                type="file"
                accept="image/*"
                multiple
                style={{ display: "none" }}
                onChange={async (e) => {

                    const filesType = Array.from(e.target.files || []);
                    const imagesOnly = filesType.filter(file =>
                        file.type.startsWith("image/")
                    );

                    if (imagesOnly.length !== filesType.length) {
                        notify({
                            ...baseOptions,
                            message: "Only image files are allowed",
                            type: "error"
                        });
                        e.target.value = "";
                        return;
                    }

                    const files = e.target.files;
                    if (!files) return;

                    const formData = new FormData();
                    Array.from(files).forEach(f => formData.append("files", f));

                    const parcelNumber = editingRow?.Parcel?.ParcelNumber;
                    if (parcelNumber) {
                        formData.append("ParcelNumber", parcelNumber);
                    }

                    const response = await fetch(`${API_URL}/landbank/uploads`, {
                        method: "POST",
                        body: formData,
                        headers: {
                            "Accept": "application/json"
                        }
                    });

                    if (!response.ok) {
                        throw new Error("Upload failed");
                    }

                    const result = await response.json();

                    if (!result.Success) {
                        notify({
                            ...baseOptions,
                            message: "Images upload failed",
                            type: "error"
                        });
                    }
                    else {
                        notify({
                            ...baseOptions,
                            message: "Images uploaded successfully",
                            type: "success"
                        });
                    }
                }}
            />

            <input
                ref={videoInputRef}
                type="file"
                accept="video/*"
                multiple
                style={{ display: "none" }}
                onChange={async (e) => {

                    const filesType = Array.from(e.target.files || []);
                    const imagesOnly = filesType.filter(file =>
                        file.type.startsWith("video/")
                    );

                    if (imagesOnly.length !== filesType.length) {
                        notify({
                            ...baseOptions,
                            message: "Only video files are allowed",
                            type: "error"
                        });
                        e.target.value = "";
                        return;
                    }

                    const files = e.target.files;
                    if (!files) return;

                    const formData = new FormData();
                    Array.from(files).forEach(f => formData.append("files", f));

                    const parcelNumber = editingRow?.Parcel?.ParcelNumber;
                    if (parcelNumber) {
                        formData.append("ParcelNumber", parcelNumber);
                    }

                    const response = await fetch(`${API_URL}/landbank/uploads`, {
                        method: "POST",
                        body: formData,
                        headers: {
                            "Accept": "application/json"
                        }
                    });

                    if (!response.ok) {
                        throw new Error("Upload failed");
                    }

                    const result = await response.json();

                    if (!result.Success) {
                        notify({
                            ...baseOptions,
                            message: "Videos upload failed",
                            type: "error"
                        });
                    }
                    else {
                        notify({
                            ...baseOptions,
                            message: "Videos uploaded successfully",
                            type: "success"
                        });
                    }
                }}
            />

        </div>
    );
}
