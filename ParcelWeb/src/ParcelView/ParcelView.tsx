
import DataGrid, { Column, Paging, Pager, FilterRow, SearchPanel, Sorting, Editing, Summary, TotalItem, ColumnChooser, StateStoring, Position, ColumnChooserSearch, ColumnChooserSelection, Scrolling, Toolbar, Item, HeaderFilter, LoadPanel } from "devextreme-react/data-grid";
import { CustomStore } from "devextreme/common/data";
import type { RowDblClickEvent } from "devextreme/ui/data_grid";
import type dxDataGrid from "devextreme/ui/data_grid";
import { useRef, useState } from "react";
import "../Parcel/Parcel.css"


export type ParcelViewProps = {
    LandBankId: number;
    Street: string;
    City: string;
    State: string;
    ZipCode: string;
    HasDemo: boolean;
    Dimensions: string;
    Notes: string;
    PermitStatus: string;
    LastDateToApply: Date;
    Acreage: number;
    SquareFoot: number;
    PropertyStatus: string;
    PropertyClassification: string;
    Owner: string;
    Source: string;
    ParcelNumber: string;
    ShortParcel: string;
    AskingPrice: number;
    UpdatedAskingPrice: number;
    IsInterested: string;
    ApplicationStatus: string;
    ApplicationNumber: string;
    SubmittedAccount: string;
    OurBid: number;
    Competitor: string;
    WinningBid: number;
    SubmitDate: Date;
    AcceptedDate: Date;
    UpperLimit: number;
    CompLimit: string;
    AdDate: Date;
    BidOffDate: Date;
    AccessorLink: string;
    GisLink: string;
    RegistryLink: string;
}

const API_URL = import.meta.env.VITE_API_URL;

let ParcelViewCache: ParcelViewProps[] = [];

const ParcelViewData = new CustomStore<ParcelViewProps, number>({
    key: "LandBankId",
    load: async () => {
        const response = await fetch(`${API_URL}/parcels?format=json`);
        if (!response.ok) throw new Error("Failed to load parcels");
        const data = await response.json();
        ParcelViewCache = data;
        return data;
    },
    byKey: async (key: number) => {
        const response = ParcelViewCache.find(p => p.LandBankId === key)
        if (!response) throw new Error(`Failed to load parcel with key ${key}`);
        return response
    }
})

const linkCell = ({ value }: any) =>
    value ? (
        <a href={value} target="_blank" onClick={(e) => e.stopPropagation()}>
            🔗
        </a>
    ) : null;


export default function ParcelView() {

    const gridRef = useRef<dxDataGrid<ParcelViewProps, number> | undefined>(null);
    const [editingRow, setEditingRow] = useState<ParcelViewProps>();

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
                    dataSource={ParcelViewData}
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
                    wordWrapEnabled={false}
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
                                                // onClick: () => {
                                                //     loadBookmark(editingRow?.Id || 0);
                                                // }
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
                                                // onClick: () => loadApplicationStatus(editingRow?.Id || 0)
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
                    <Column dataField="LandBankId" dataType="number"></Column>
                    <Column dataField="Street" dataType="string"></Column>
                    <Column dataField="City" dataType="string"></Column>
                    <Column dataField="State" dataType="string"></Column>
                    <Column dataField="ZipCode" dataType="number"></Column>
                    <Column dataField="HasDemo" dataType="boolean"></Column>
                    <Column dataField="Dimensions" dataType="string"></Column>
                    <Column dataField="Notes" dataType="string"></Column>
                    <Column dataField="PermitStatus" dataType="string"></Column>
                    <Column dataField="LastDateToApply" dataType="date"></Column>
                    <Column dataField="Acreage" dataType="number"></Column>
                    <Column dataField="SquareFoot" dataType="number"></Column>
                    <Column dataField="PropertyStatus" dataType="string"></Column>
                    <Column dataField="PropertyClassification" dataType="string"></Column>
                    <Column dataField="Owner" dataType="string"></Column>
                    <Column dataField="Source" dataType="string"></Column>
                    <Column dataField="ParcelNumber" dataType="string"></Column>
                    <Column dataField="ShortParcel" dataType="string"></Column>
                    <Column dataField="AskingPrice" dataType="number"></Column>
                    <Column dataField="UpdatedAskingPrice" dataType="number"></Column>
                    <Column dataField="IsInterested" dataType="string"></Column>
                    <Column dataField="ApplicationStatus" dataType="string"></Column>
                    <Column dataField="ApplicationNumber" dataType="string"></Column>
                    <Column dataField="SubmittedAccount" dataType="string"></Column>
                    <Column dataField="OurBid" dataType="number"></Column>
                    <Column dataField="Competitor" dataType="string"></Column>
                    <Column dataField="WinningBid" dataType="number"></Column>
                    <Column dataField="SubmitDate" dataType="date"></Column>
                    <Column dataField="AcceptedDate" dataType="date"></Column>
                    <Column dataField="UpperLimit" dataType="number"></Column>
                    <Column dataField="CompLimit" dataType="string"></Column>
                    <Column dataField="AdDate" dataType="date"></Column>
                    <Column dataField="BidOffDate" dataType="date"></Column>
                    <Column dataField="AccessorLink" dataType="string" width={100} cellRender={linkCell}></Column>
                    <Column dataField="GisLink" dataType="string" width={100} cellRender={linkCell}></Column>
                    <Column dataField="RegistryLink" dataType="string" width={150} cellRender={linkCell}></Column>
                </DataGrid>
            </main>
            {/* Footer */}
            <footer className="parcel-footer">
                © {new Date().getFullYear()} Parcel Management System · Internal Use Only
            </footer>
        </div>
    );
}