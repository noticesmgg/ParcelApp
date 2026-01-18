import DataGrid, { Column, Paging, Pager, FilterRow, SearchPanel, Sorting, Editing, Summary, TotalItem, ColumnChooser, StateStoring, Position, ColumnChooserSearch, ColumnChooserSelection, Scrolling, Toolbar, Item, HeaderFilter, LoadPanel, type DataGridTypes } from "devextreme-react/data-grid";
import CustomStore from "devextreme/data/custom_store";
import "devextreme/dist/css/dx.light.css";
import "./Parcel.css";
import notify from "devextreme/ui/notify";
import type dxDataGrid from "devextreme/ui/data_grid";
import { useRef, useState } from "react";
import type { EditingStartEvent, RowDblClickEvent, RowPreparedEvent } from "devextreme/ui/data_grid";
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

export interface ParcelsDO {
    LandBankId: number;
    Street?: string;
    City?: string;
    State?: string;
    ZipCode?: number;
    HasDemo?: boolean;
    Dimensions?: string;
    Notes?: string;
    PermitStatus?: string;
    LastDateToApply?: Date | string;
    Acreage?: number;
    SquareFoot?: number;
    PropertyStatus?: string;
    PropertyClassification?: string;
    Owner?: string;
    Source?: string;
    ParcelNumber?: string;
    ShortParcel?: string;
    AskingPrice?: number;
    UpdatedAskingPrice?: number;
    IsInterested?: string;
    ApplicationStatus?: string;
    ApplicationNumber?: string;
    SubmittedAccount?: string;
    OurBid?: number;
    Competitor?: string;
    WinningBid?: number;
    SubmitDate?: Date | string | null;
    AcceptedDate?: Date | string | null;
    UpperLimit?: number;
    CompLimit?: string;
    AdDate?: Date | string;
    BidOffDate?: Date | string;
    LandAppraisal?: number;
    BuildingAppraisal?: number;
    TotalAppraisal?: number;
    TotalAssessment?: number;
    LandUse?: string;
    YearBuilt?: number;
    Stories?: number;
    TotalRooms?: number;
    RecentDateOfSale?: Date | string | null;
    RecentSalesPrice?: number;
    SecondRecentDateOfSale?: Date | string | null;
    SecondRecentSalesPrice?: number;
    AccessorLink?: string;
    GisLink?: string;
    RegistryLink?: string;
}

export interface ParcelMedia {
    ImagesUrl: string[];
    VideosUrl: string[];
}

const API_URL = import.meta.env.VITE_API_URL;
let parcelCache: ParcelsDO[] = [];

const parcelStore = new CustomStore<ParcelsDO, number>({
    key: "LandBankId",
    load: async () => {
        if (parcelCache.length > 0) {
            return parcelCache;
        }
        const response = await fetch(`${API_URL}/parcels?format=json`);
        if (!response.ok) throw new Error("Failed to load parcels");
        const data = await response.json();
        parcelCache = data;
        return data;
    },
    byKey: async (key) => {
        const parcel = parcelCache.find(p => p.LandBankId === key);
        if (!parcel) throw new Error("Parcel not found");
        return parcel;
    },
    update: async (key, values) => {
        const index = parcelCache.findIndex(p => p.LandBankId === key);
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

            parcelCache = parcelCache.filter(p => p.LandBankId !== key);

            notify({ ...baseOptions, message: "Parcel deleted", type: "success" });
        } catch (err) {
            notify({ ...baseOptions, message: "Delete failed", type: "error" });
            throw err;
        }
    }
});

export default function ParcelGrid() {

    const gridRef = useRef<dxDataGrid<ParcelsDO, number> | undefined>(null);
    const [editingRow, setEditingRow] = useState<ParcelsDO>();
    const imageInputRef = useRef<HTMLInputElement>(null);
    const videoInputRef = useRef<HTMLInputElement>(null);
    const [hasDemoOn, setHasDemoOn] = useState(false);
    const [todayOn, setTodayOn] = useState(false);


    const OnRowDoubleClick = (e: RowDblClickEvent) => {
        if (e.rowIndex === undefined || e.rowType !== "data") return;

        gridRef.current?.editRow(e.rowIndex);
    }

    const OnEditingStart = async (e: EditingStartEvent<ParcelsDO, number>) => {
        const parcel = e.data as ParcelsDO;
        setEditingRow(parcel);
    }

    const applyFilters = (hasDemo: boolean, today: boolean) => {
        const filters = [];

        if (hasDemo) {
            filters.push(["HasDemo", "=", true]);
        }

        if (today) {
            const start = new Date();
            start.setHours(0, 0, 0, 0);

            const dateFilter = [
                ["LastDateToApply", ">=", start]
            ];

            if (filters.length) {
                filters.push("and", dateFilter);
            } else {
                filters.push(dateFilter);
            }
        }

        if (filters.length) {
            gridRef.current?.option("filterValue", filters);
        } else {
            gridRef.current?.clearFilter();
        }
    };

    const OnRowPrepared = (e: RowPreparedEvent<ParcelsDO, number>) => {
        if (e.rowType !== "data") return;

         e.rowElement.style.backgroundColor = "";
         e.rowElement.style.fontWeight = "";

        const parcel = e.data as ParcelsDO;

        const appNo = parcel.ApplicationNumber;
        const status = parcel.ApplicationStatus;

        const hasAppNo = appNo !== null && appNo !== undefined && String(appNo).trim() !== "";
        const hasStatus = status !== null && status !== undefined && String(status).trim() !== "";

        if (hasAppNo && hasStatus) {
            e.rowElement.style.backgroundColor = "#FBE2D5"; // light orange
            //e.rowElement.style.fontWeight = "600"; // optional
        }
        };

    const onToolbarPreparing = (e: DataGridTypes.ToolbarPreparingEvent) => {
        e.toolbarOptions.items?.unshift(
            {
                location: "after",
                widget: "dxButton",
                options: {
                    text: "Has Demo",
                    icon: "check",
                    type: hasDemoOn ? "default" : "normal",
                    stylingMode: hasDemoOn ? "contained" : "outlined",
                    onClick: () => {
                        const next = !hasDemoOn;
                        setHasDemoOn(next);
                        applyFilters(next, todayOn);
                    }
                }
            },
            {
                location: "after",
                widget: "dxButton",
                options: {
                    text: "Last Apply Date",
                    icon: "event",
                    type: todayOn ? "default" : "normal",
                    stylingMode: todayOn ? "contained" : "outlined",
                    onClick: () => {
                        const next = !todayOn;
                        setTodayOn(next);
                        applyFilters(hasDemoOn, next);
                    }
                }
            }
        );
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
                    repaintChangesOnly={false}
                    allowColumnReordering={true}
                    allowColumnResizing={true}
                    columnResizingMode="widget"
                    showColumnLines={true}
                    showRowLines={true}
                    wordWrapEnabled={false}
                    columnHidingEnabled={false}
                    onRowDblClick={OnRowDoubleClick}
                    onEditingStart={OnEditingStart}
                    onToolbarPreparing={onToolbarPreparing}
                    onRowPrepared={OnRowPrepared}
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
                            title: editingRow?.Street
                                ? `Edit Parcel — ${editingRow.Street}`
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
                                            { dataField: "Street" },
                                            { dataField: "City" },
                                            { dataField: "State" },
                                            { dataField: "ZipCode" },
                                            { dataField: "AskingPrice", editorOptions: { format: { type: "currency", precision: 0 }, stylingMode: "outlined" } },
                                            { dataField: "UpdatedAskingPrice", editorOptions: { format: { type: "currency", precision: 0 }, stylingMode: "outlined" } },
                                            { dataField: "Acreage" },
                                            { dataField: "SquareFoot" },
                                            { dataField: "Dimensions" },
                                            { dataField: "Source" },
                                            { dataField: "Owner" }
                                        ]
                                    },
                                    {
                                        title: "Pricing",
                                        colCount: 2,
                                        items: [
                                            { dataField: "LandAppraisal" },
                                            { dataField: "BuildingAppraisal" },
                                            { dataField: "TotalAppraisal" },
                                            { dataField: "TotalAssessment" },
                                            { dataField: "LandUse" },
                                            { dataField: "YearBuilt" },
                                            { dataField: "Stories" },
                                            { dataField: "TotalRooms" },
                                            { dataField: "RecentDateOfSale" },
                                            { dataField: "RecentSalesPrice" },
                                            { dataField: "SecondRecentDateOfSale" },
                                            { dataField: "SecondRecentSalesPrice" }
                                        ]
                                    },
                                    {
                                        title: "Dates & Status",
                                        colCount: 2,
                                        items: [
                                            { dataField: "AdDate", editorType: "dxDateBox" },
                                            { dataField: "BidOffDate", editorType: "dxDateBox" },
                                            { dataField: "LastDateToApply", editorType: "dxDateBox" },
                                            { dataField: "HasDemo" },
                                            { dataField: "LandUse" },
                                            { dataField: "PropertyStatus" },
                                            { dataField: "PropertyClassification" },
                                        ]
                                    },
                                    {
                                        title: "Bookmark Status",
                                        colCount: 2,
                                        items: [
                                            { dataField: "IsInterested", editorType: "dxSelectBox", editorOptions: { items: ["Yes", "No", "Conditional", "Pending", "Ignore"] } },
                                            { dataField: "UpperLimit", editorType: "dxNumberBox", editorOptions: { format: { type: "currency", precision: 0 }, stylingMode: "outlined" } },
                                            { dataField: "CompLimit", editorType: "dxTextBox", editorOptions: { format: { type: "currency", precision: 0 }, stylingMode: "outlined" } },
                                            { dataField: "Notes", editorType: "dxTextBox" },
                                        ]
                                    },
                                    {
                                        title: "Application Status",
                                        colCount: 2,
                                        items: [
                                            { dataField: "SubmittedAccount", editorType: "dxTextBox", },
                                            { dataField: "SubmitDate", editorType: "dxDateBox", },
                                            { dataField: "ReSubmitDate", editorType: "dxDateBox", },
                                            { dataField: "AcceptedDate", editorType: "dxDateBox", },
                                            { dataField: "ApplicationNumber", editorType: "dxTextBox", },
                                            { dataField: "ApplicationStatus", editorType: "dxSelectBox", editorOptions: { items: ["Bid Submitted", "Bid Accepted", "Pending", "Reverted", "Re-Submitted", "Accepted", "Bid Lost", "Bid Won"], stylingMode: "outlined" } },
                                            { dataField: "OurBid", editorType: "dxNumberBox", editorOptions: { format: "currency" } },
                                            { dataField: "Competitor", editorType: "dxTextBox", },
                                            { dataField: "WinningBid", editorType: "dxNumberBox", editorOptions: { format: "currency" } },
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
                                    },
                                    {
                                        title: "Identifiers",
                                        colCount: 2,
                                        items: [
                                            { dataField: "LandBankId", editorOptions: { readOnly: true } },
                                            { dataField: "ParcelNumber", editorOptions: { readOnly: true } },
                                            { dataField: "ShortParcel", editorOptions: { readOnly: true } },
                                        ]
                                    },

                                ]
                            }]
                        }}
                    />

                    <Column dataField="LandBankId" caption="ID" dataType="number" width={80} visible={false} />
                    <Column dataField="Street" caption="Street" dataType="string" width={200} />
                    <Column dataField="City" caption="City" dataType="string" width={120} />
                    <Column dataField="State" caption="State" dataType="string" width={80} />
                    <Column dataField="ZipCode" caption="Zip Code" dataType="number" width={100} />
                    <Column dataField="HasDemo" caption="Has Demo" dataType="boolean" width={100} />
                    <Column dataField="Dimensions" caption="Dimensions" dataType="string" width={150} />
                    <Column dataField="Notes" caption="Notes" dataType="string" width={250} />
                    <Column dataField="PermitStatus" caption="Permit Status" dataType="string" width={140} />
                    <Column dataField="LastDateToApply" caption="Last Date To Apply" dataType="date" width={150} />
                    <Column dataField="Acreage" caption="Acreage" dataType="number" width={100} format={{ type: "fixedPoint", precision: 3 }}/>
                    <Column dataField="SquareFoot" caption="Square Foot" dataType="number" width={120} format={{ type: "fixedPoint", precision: 0 }}/>
                    <Column dataField="PropertyStatus" caption="Property Status" dataType="string" width={150} />
                    <Column dataField="PropertyClassification" caption="Property Classification" dataType="string" width={200} />
                    <Column dataField="Owner" caption="Owner" dataType="string" width={200} />
                    <Column dataField="Source" caption="Source" dataType="string" width={120} />
                    <Column dataField="ParcelNumber" caption="Parcel Number" dataType="string" width={160} fixed fixedPosition="left" />
                    <Column dataField="ShortParcel" caption="Short Parcel" dataType="string" width={140} fixed fixedPosition="left" />
                    <Column dataField="AskingPrice" caption="Asking Price" dataType="number" format="currency" width={130} />
                    <Column dataField="UpdatedAskingPrice" caption="Updated Asking Price" dataType="number" format="currency" width={170} />
                    <Column dataField="IsInterested" caption="Interested" dataType="string" width={120} />
                    <Column dataField="ApplicationStatus" caption="Application Status" dataType="string" width={160} />
                    <Column dataField="ApplicationNumber" caption="Application Number" dataType="string" width={170} />
                    <Column dataField="SubmittedAccount" caption="Submitted Account" dataType="string" width={160} />
                    <Column dataField="OurBid" caption="Our Bid" dataType="number" format="currency" width={120} />
                    <Column dataField="Competitor" caption="Competitor" dataType="string" width={140} />
                    <Column dataField="WinningBid" caption="Winning Bid" dataType="number" format="currency" width={130} />
                    <Column dataField="SubmitDate" caption="Submit Date" dataType="date" width={130} />
                    <Column dataField="ReSubmitDate" caption="ReSubmit Date" dataType="date" width={130} />
                    <Column dataField="AcceptedDate" caption="Accepted Date" dataType="date" width={150} />
                    <Column dataField="UpperLimit" caption="Upper Limit" dataType="number" format="currency" width={130} />
                    <Column dataField="CompLimit" caption="Comp Limit" dataType="string" width={120} />
                    <Column dataField="AdDate" caption="Ad Date" dataType="date" width={120} />
                    <Column dataField="BidOffDate" caption="Bid Off Date" dataType="date" width={140} />
                    <Column dataField="LandAppraisal" caption="Land Appraisal" dataType="number" format="currency" width={130} />
                    <Column dataField="BuildingAppraisal" caption="Building Appraisal" dataType="number" format="currency" width={150} />
                    <Column dataField="TotalAppraisal" caption="Total Appraisal" dataType="number" format="currency" width={150} />
                    <Column dataField="TotalAssessment" caption="TotalAssessment" dataType="number" format="currency" width={150} />
                    <Column dataField="LandUse" caption="Land Use" dataType="string" width={150} />
                    <Column dataField="YearBuilt" caption="Year Built" dataType="number" width={100} />
                    <Column dataField="Stories" caption="Stories" dataType="number" width={100} />
                    <Column dataField="TotalRooms" caption="Total Rooms" dataType="number" width={120} />
                    <Column dataField="RecentDateOfSale" caption="Recent Date Of Sale" dataType="date" width={150} />
                    <Column dataField="RecentSalesPrice" caption="Recent Sales Price" dataType="number" format="currency" width={150} />
                    <Column dataField="SecondRecentDateOfSale" caption="Second Recent Date Of Sale" dataType="date" width={200} />
                    <Column dataField="SecondRecentSalesPrice" caption="Second Recent Sales Price" dataType="number" format="currency" width={200} />
                    <Column
                        dataField="AccessorLink"
                        caption="Accessor Link"
                        dataType="string"
                        width={200}
                        cellRender={(cellData: any) => {
                            const value = cellData?.value as string | undefined;
                            if (!value) return <span />;
                            const href = value.startsWith("http") ? value : `https://${value}`;
                            return (
                                <a href={href} target="_blank" rel="noopener noreferrer">
                                    {value}
                                </a>
                            );
                        }}
                    />
                    <Column
                        dataField="GisLink"
                        caption="GIS Link"
                        dataType="string"
                        width={200}
                        cellRender={(cellData: any) => {
                            const value = cellData?.value as string | undefined;
                            if (!value) return <span />;
                            const href = value.startsWith("http") ? value : `https://${value}`;
                            return (
                                <a href={href} target="_blank" rel="noopener noreferrer">
                                    {value}
                                </a>
                            );
                        }}
                    />
                     <Column
                        dataField="RegistryLink"
                        caption="Registry Link"
                        dataType="string"
                        width={200}
                        cellRender={(cellData: any) => {
                            const value = cellData?.value as string | undefined;
                            if (!value) return <span />;
                            const href = value.startsWith("http") ? value : `https://${value}`;
                            return (
                                <a href={href} target="_blank" rel="noopener noreferrer">
                                    {value}
                                </a>
                            );
                        }}
                    />
                    <Summary >
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
                    try {
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
                            return;
                        }

                        const files = e.target.files;
                        if (!files) return;

                        const formData = new FormData();
                        Array.from(files).forEach(f => formData.append("files", f));

                        const landBankId = editingRow?.LandBankId;
                        if (landBankId) {
                            formData.append("LandBankId", landBankId.toString());
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

                        notify({
                            ...baseOptions,
                            message: result.FailedFiles.length === 0
                                ? "Images uploaded successfully"
                                : "Images upload failed",
                            type: result.FailedFiles.length === 0 ? "success" : "error"
                        });
                    }
                    catch (err) {
                        console.error(err);
                        notify({
                            ...baseOptions,
                            message: "Upload failed",
                            type: "error"
                        });
                    }
                    finally {
                        imageInputRef.current!.value = "";
                        e.target.value = "";
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
                    try {
                        const filesType = Array.from(e.target.files || []);
                        const videosOnly = filesType.filter(file =>
                            file.type.startsWith("video/")
                        );

                        if (videosOnly.length !== filesType.length) {
                            notify({
                                ...baseOptions,
                                message: "Only video files are allowed",
                                type: "error"
                            });
                            return;
                        }

                        const files = e.target.files;
                        if (!files) return;

                        const formData = new FormData();
                        Array.from(files).forEach(f => formData.append("files", f));

                        const parcelNumber = editingRow?.ParcelNumber;
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

                        notify({
                            ...baseOptions,
                            message: result.FailedFiles.length === 0
                                ? "Videos uploaded successfully"
                                : "Videos upload failed",
                            type: result.FailedFiles.length === 0 ? "success" : "error"
                        });
                    }
                    catch (err) {
                        console.error(err);
                        notify({
                            ...baseOptions,
                            message: "Video upload failed",
                            type: "error"
                        });
                    }
                    finally {
                        e.target.value = "";
                        videoInputRef.current!.value = "";
                    }
                }}
            />
        </div>
    );
}
