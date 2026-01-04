import DataGrid, { Column, Paging, Pager, FilterRow, SearchPanel, Sorting, Editing, Summary, TotalItem, ColumnChooser, StateStoring, Position, ColumnChooserSearch, ColumnChooserSelection, Scrolling, Toolbar, Item, HeaderFilter, LoadPanel } from "devextreme-react/data-grid";
import Popup from "devextreme-react/popup";
import Form from "devextreme-react/form";
import CustomStore from "devextreme/data/custom_store";
import "devextreme/dist/css/dx.light.css";
import "./Parcel.css";
import notify from "devextreme/ui/notify";
import type dxDataGrid from "devextreme/ui/data_grid";
import { useRef, useState } from "react";
import type { RowDblClickEvent } from "devextreme/ui/data_grid";

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

// const API_URL = "http://localhost:9867";
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
    const [bookmarkVisible, setBookmarkVisible] = useState(false);
    const [applicationStatusVisible, setApplicationStatusVisible] = useState(false);
    const [editingRow, setEditingRow] = useState<ParcelDO>();
    const [applicationStatus, setApplicationStatus] = useState<ApplicationStatusDO>();
    const [bookmark, setBookmark] = useState<BookmarkDO>();

    const loadApplicationStatus = async (landBankId: number) => {
        try {
            const response = await fetch(`${API_URL}/lbapplicationstatus/?format=json&LandBankId=${landBankId}`);
            if (!response.ok) throw new Error("Failed to load application status");
            const data = await response.json();
            setApplicationStatus(data);
            setApplicationStatusVisible(true);
        }
        catch (err) {
            notify({ ...baseOptions, message: "Failed to load application status", type: "error" });
            throw err;
        }
    };

    const loadBookmark = async (landBankId: number) => {
        try {
            const response = await fetch(`${API_URL}/lbbookmarks/?format=json&LandBankId=${landBankId}`);
            if (!response.ok) throw new Error("Failed to load bookmark");
            const data = await response.json();
            setBookmark(data);
            setBookmarkVisible(true);
        }
        catch (err) {
            notify({ ...baseOptions, message: "Failed to load bookmark", type: "error" });
            throw err;
        }
    };

    // useEffect(() => {
    //     if (bookmark) {
    //         setBookmarkVisible(true);
    //     }
    // }, [bookmark]);

    const OnRowDoubleClick = (e: RowDblClickEvent) => {
        if (e.rowIndex === undefined || e.rowType !== "data") return;

        gridRef.current?.editRow(e.rowIndex);
    }


    return (
        <div className="parcel-app">
            <header className="parcel-header">
                <div className="parcel-header-left">
                    <h1>Parcel Management</h1>
                    <span className="subtitle">Land Bank & Property Records</span>
                </div>
                <div className="parcel-header-right">
                    <span className="env">DEV</span>
                </div>
            </header>

            <main className="parcel-content">
                <DataGrid
                    onInitialized={(e) => (gridRef.current = e.component)}
                    dataSource={parcelStore}
                    remoteOperations={false}
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
                    onRowDblClick={OnRowDoubleClick}
                    onEditingStart={(e) => setEditingRow(e.data)}
                    wordWrapEnabled={true}
                    columnHidingEnabled={true}
                    scrolling={{ mode: "virtual" }}
                >
                    <Toolbar>
                        <Item name="searchPanel" location="before" />
                        <Item name="columnChooserButton" location="after" />
                    </Toolbar>
                    <Scrolling columnRenderingMode="virtual" rowRenderingMode="virtual" />
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
                        allowUpdating={true}
                        allowDeleting={true}
                        allowAdding={false}
                        popup={{
                            title: "Edit Parcel",
                            showTitle: true,
                            width: 600,
                            maxHeight: "90vh",
                            shading: true,
                            shadingColor: "rgba(0, 0, 0, 0.35)",
                            hideOnOutsideClick: false,
                            //focusStateEnabled: true,
                            position: { my: "center", at: "center", of: window }
                        }}
                        form={{
                            colCount: 2,
                            labelLocation: "top",
                            items: [
                                {
                                    itemType: "group",
                                    colSpan: 2,
                                    colCount: 2,
                                    cssClass: "form-action-buttons",
                                    items: [
                                        {
                                            itemType: "button",
                                            horizontalAlignment: "right",
                                            buttonOptions: {
                                                text: "Bookmark",
                                                icon: "bookmark",
                                                stylingMode: "outlined",
                                                type: "default",
                                                hint: "Bookmark this parcel",
                                                onClick: () => {
                                                    loadBookmark(editingRow?.Id || 0);
                                                }
                                            }
                                        },
                                        {
                                            itemType: "button",
                                            horizontalAlignment: "left",
                                            buttonOptions: {
                                                text: "Application Status",
                                                icon: "info",
                                                stylingMode: "outlined",
                                                type: "default",
                                                hint: "View application status",
                                                width: 230,
                                                onClick: () => loadApplicationStatus(editingRow?.Id || 0)
                                            }
                                        }
                                    ]
                                },
                                { dataField: "Id" },
                                { dataField: "ParcelNumber" },
                                { dataField: "ShortParcel" },
                                { dataField: "Street" },
                                { dataField: "City" },
                                { dataField: "State" },
                                { dataField: "ZipCode" },
                                { dataField: "AskingPrice", editorOptions: { format: "currency" } },
                                { dataField: "UpdatedAskingPrice", editorOptions: { format: "currency" } },
                                { dataField: "AdDate" },
                                { dataField: "BidOffDate" },
                                { dataField: "LastDateToApply" },
                                { dataField: "Acreage" },
                                { dataField: "SquareFoot" },
                                { dataField: "Dimensions" },
                                { dataField: "HasDemo" },
                                { dataField: "PermitStatus" },
                                { dataField: "PropertyStatus" },
                                { dataField: "PropertyClassification" },
                                { dataField: "Source" },
                                { dataField: "Owner" },
                                { dataField: "CreatedDate" },
                                { dataField: "UpdatedDate" },
                                { dataField: "CreatedBy" },
                                { dataField: "UpdatedBy" },
                            ]
                        }}
                    />
                    <Column dataField="Id" visible={false} allowEditing={false} dataType="number" caption="ID" />
                    <Column dataField="ParcelNumber" caption="Parcel #" allowEditing={false} fixed fixedPosition="left" />
                    <Column dataField="ShortParcel" caption="Short Parcel" fixed fixedPosition="left" />
                    <Column dataField="Street" />
                    <Column dataField="City" />
                    <Column dataField="State" width={80} />
                    <Column dataField="ZipCode" caption="ZIP" width={100} />
                    <Column dataField="AskingPrice" caption="Asking Price" dataType="number" format="currency" />
                    <Column dataField="UpdatedAskingPrice" caption="Updated Price" dataType="number" format="currency" />
                    <Column dataField="AdDate" caption="Ad Date" dataType="date" />
                    <Column dataField="BidOffDate" caption="Bid Off Date" dataType="date" />
                    <Column dataField="LastDateToApply" caption="Last Apply Date" dataType="date" />
                    <Column dataField="Acreage" caption="Acreage" dataType="number" />
                    <Column dataField="SquareFoot" caption="Sq Ft" dataType="number" />
                    <Column dataField="Dimensions" />
                    <Column dataField="HasDemo" caption="Has Demo" dataType="boolean" />
                    <Column dataField="PermitStatus" caption="Permit Status" />
                    <Column dataField="PropertyStatus" caption="Status" />
                    <Column dataField="PropertyClassification" caption="Classification" />
                    <Column dataField="Source" />
                    <Column dataField="Owner" />
                    <Column dataField="CreatedDate" caption="Created" dataType="datetime" visible={false} allowEditing={false} />
                    <Column dataField="UpdatedDate" caption="Updated" dataType="datetime" visible={false} allowEditing={false} />
                    <Column dataField="CreatedBy" visible={false} allowEditing={false} />
                    <Column dataField="UpdatedBy" visible={false} allowEditing={false} />
                    <Summary>
                        <TotalItem
                            column="Acreage"
                            summaryType="sum"
                            valueFormat={{ type: "fixedPoint", precision: 2 }}
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

                <Popup
                    visible={bookmarkVisible}
                    title="Bookmark Parcel"
                    wrapperAttr={{ class: "child-popup" }}
                    width={600}
                    showCloseButton={true}
                    showTitle={true}
                    shading={true}
                    shadingColor="rgba(0,0,0,0.35)"
                    hideOnOutsideClick={false}
                    //focusStateEnabled={true}
                    position={{ my: "center", at: "center", of: window }}
                    onHiding={() => setBookmarkVisible(false)}
                >
                    <Form
                        formData={bookmark}
                        labelLocation="top"
                        colCount={2}
                        items={[
                            { dataField: "Id", editorType: "dxNumberBox", editorOptions: { readOnly: true } },
                            { dataField: "LandBankId", editorType: "dxNumberBox", editorOptions: { readOnly: true } },
                            { dataField: "Interest", editorType: "dxSelectBox", editorOptions: { items: ["Yes", "No", "Conditional", "Pending", "Ignore"] } },
                            { dataField: "UpperLimit", editorType: "dxNumberBox" },
                            { dataField: "CompLimit", editorType: "dxTextBox" },
                            { dataField: "Notes", editorType: "dxTextBox" },
                            { dataField: "CreatedDate", editorType: "dxDateBox", editorOptions: { readOnly: true } },
                            { dataField: "UpdatedDate", editorType: "dxDateBox", editorOptions: { readOnly: true } },
                            { dataField: "CreatedBy", editorType: "dxTextBox", editorOptions: { readOnly: true } },
                            { dataField: "UpdatedBy", editorType: "dxTextBox", editorOptions: { readOnly: true } },
                            {
                                itemType: "button",
                                horizontalAlignment: "right",
                                colSpan: 2,
                                buttonOptions: {
                                    text: "Save Bookmark",
                                    type: "success",
                                    icon: "save",
                                    onClick: () => {
                                        setBookmarkVisible(false);
                                    }
                                }
                            },
                        ]}
                    />
                </Popup>

                <Popup
                    visible={applicationStatusVisible}
                    title="Application Status"
                    wrapperAttr={{ class: "child-popup" }}
                    width={600}
                    showCloseButton={true}
                    showTitle={true}
                    shading={true}
                    shadingColor="rgba(0,0,0,0.35)"
                    hideOnOutsideClick={false}
                    //focusStateEnabled={true}
                    position={{ my: "center", at: "center", of: window }}
                    onHiding={() => setApplicationStatusVisible(false)}
                >
                    <Form
                        formData={applicationStatus}
                        labelLocation="top"
                        colCount={2}
                        items={[
                            {
                                dataField: "Id",
                                editorType: "dxNumberBox",
                                editorOptions: { readOnly: true }
                            },
                            {
                                dataField: "LandBankId",
                                editorType: "dxNumberBox",
                                editorOptions: { readOnly: true }
                            },
                            {
                                dataField: "AccountId",
                                editorType: "dxTextBox",
                            },
                            {
                                dataField: "SubmitDate",
                                editorType: "dxDateBox",
                            },
                            {
                                dataField: "ReSubmitDate",
                                editorType: "dxDateBox",
                            },
                            {
                                dataField: "AcceptedDate",
                                editorType: "dxDateBox",
                            },
                            {
                                dataField: "ApplicationNumber",
                                editorType: "dxTextBox",
                            },
                            {
                                dataField: "Status",
                                editorType: "dxSelectBox",
                                editorOptions: {
                                    items: ["Bid Submitted", "Bid Accepted", "Pending", "Reverted", "Re-Submitted", "Accepted", "Bid Lost", "Bid Won"]
                                }
                            },
                            {
                                dataField: "OurBid",
                                editorType: "dxNumberBox",
                                editorOptions: { format: "currency" }
                            },
                            {
                                dataField: "Competitor",
                                editorType: "dxTextBox",
                            },
                            {
                                dataField: "WinningBid",
                                editorType: "dxNumberBox",
                                editorOptions: { format: "currency" }
                            },
                            {
                                dataField: "CreatedDate",
                                editorType: "dxDateBox",
                                editorOptions: { readOnly: true }
                            },
                            {
                                dataField: "UpdatedDate",
                                editorType: "dxDateBox",
                                editorOptions: { readOnly: true }
                            },
                            { dataField: "CreatedBy", editorType: "dxTextBox", editorOptions: { readOnly: true } },
                            { dataField: "UpdatedBy", editorType: "dxTextBox", editorOptions: { readOnly: true } },
                            {
                                itemType: "button",
                                horizontalAlignment: "right",
                                colSpan: 2,
                                buttonOptions: {
                                    text: "Save Status",
                                    type: "success",
                                    icon: "save",
                                    onClick: () => {
                                        setApplicationStatusVisible(false);
                                    }
                                }
                            },
                        ]}
                    />
                </Popup>
            </main>

            {/* Footer */}
            <footer className="parcel-footer">
                © {new Date().getFullYear()} Parcel Management System · Internal Use Only
            </footer>
        </div>
    );
}
